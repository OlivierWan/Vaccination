using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Vaccination.Application.Dtos.Authentication;
using Vaccination.Application.Exceptions;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Shared;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;

namespace Vaccination.Application.Services
{
    public class AuthService(UserManager<User> userManager, IConfiguration configuration, IUnitOfWork unitOfWork) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            User? user = await _userManager.FindByEmailAsync(loginRequest.Email) ?? throw new NotFoundException("Utilisateur inconnu");

            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (!isPasswordCorrect)
            {
                throw new PasswordException("Mot de passe incorrect");
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            List<Claim> authClaims =
            [
               // new Claim(ClaimTypes.Email, user.Email),
               // new Claim(ClaimTypes.NameIdentifier, user.Id),
                new("email", user.Email ?? string.Empty),
                new("userid", user.Id ?? string.Empty),
                new("jwtid", Guid.NewGuid().ToString()),
                new("firstname", user.FirstName ?? string.Empty),
                new("lastname", user.LastName ?? string.Empty),
            ];

            foreach (string userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            JwtSecurityToken token = GenerateNewJsonWebToken(authClaims);
            string refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshToken"], out int refreshTokenValidityInDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            IdentityResult updateResult = await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            if (!updateResult.Succeeded)
            {
                throw new DatabaseException("Échec de la mise à jour du jeton de rafraîchissement");
            }

            return new LoginResponse(
               Expiration: token.ValidTo,
               RefreshToken: refreshToken,
               Token: new JwtSecurityTokenHandler().WriteToken(token)
           );
        }

        public async Task<TokenResponse> RefreshTokenAsync(TokenRequest tokenRequest)
        {
            string accessToken = tokenRequest.AccessToken;
            string refreshToken = tokenRequest.RefreshToken;

            ClaimsPrincipal? principal = GetPrincipalFromExpiredToken(accessToken) ?? throw new TokenException("ClaimsPrincipal non trouvé dans le JWT Token");

            if (principal.Identity is null)
            {
                throw new TokenException("JWT Refresh token invalide");
            }

            string email = principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

            User? user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException("Utilisateur non trouvé");

            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new TokenException("JWT Refresh token invalide");
            }

            JwtSecurityToken newAccessToken = GenerateNewJsonWebToken(principal.Claims.ToList());
            string newRefreshToken = GenerateRefreshToken();



            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            return new TokenResponse(
               Token: new JwtSecurityTokenHandler().WriteToken(newAccessToken),
               RefreshToken: newRefreshToken,
               Expiration: newAccessToken.ValidTo
           );
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            User? isExistsUser = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (isExistsUser != null)
            {
                throw new DuplicateDataException("Utilisateur déjà existant");
            }
            User newUser = new()
            {
                Email = registerRequest.Email,
                UserName = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            IdentityResult createUserResult = await _userManager.CreateAsync(newUser, registerRequest.Password);
            if (!createUserResult.Succeeded)
            {
                StringBuilder errorStringBuilder = new("Échec de la création de l'utilisateur en raison de : ");
                foreach (IdentityError error in createUserResult.Errors)
                {
                    errorStringBuilder.Append(" # ").Append(error.Description);
                }

                throw new DatabaseException(errorStringBuilder.ToString());

            }
            await _userManager.AddToRoleAsync(newUser, RolesConstants.USER);
            await _userManager.AddToRoleAsync(newUser, RolesConstants.READ);
            await _userManager.AddToRoleAsync(newUser, RolesConstants.WRITE);
            await _userManager.AddToRoleAsync(newUser, RolesConstants.DELETE);

            return new RegisterResponse();
        }

        private static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[64];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private static ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            string? jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new TokenException("Le secret JWT n'est pas configuré correctement.");
            }

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("JWT Token invalide");

            return principal;
        }

        private JwtSecurityToken GenerateNewJsonWebToken(List<Claim> claims)
        {
            string? jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new TokenException("Le secret JWT n'est pas configuré correctement.");
            }

            SymmetricSecurityKey authSecret = new(Encoding.UTF8.GetBytes(jwtSecret));

            _ = int.TryParse(_configuration["JWT:TokenValidity"], out int tokenValidity);
            DateTime dateTime = DateTime.Now.AddHours(tokenValidity);

            JwtSecurityToken tokenObject = new(

                     issuer: _configuration["JWT:ValidIssuer"],
                     audience: _configuration["JWT:ValidAudience"],
                     expires: dateTime,
                     claims: claims,
                     signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                 );

            return tokenObject;
        }
    }
}