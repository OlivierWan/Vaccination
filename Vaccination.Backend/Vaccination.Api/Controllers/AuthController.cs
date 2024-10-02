
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vaccination.Application.Dtos;
using Vaccination.Application.Dtos.Authentication;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Validators.Auth;

namespace Vaccination.Api.Controllers
{
    /// <summary>
    /// Controller for managing authentication.
    /// </summary>
    [AllowAnonymous]
    public class AuthController(IAuthService authService) : BaseController
    {
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Authenticate a user and returns a JWT token.
        /// </summary>
        /// <param name="loginRequest">The login request containing user credentials.</param>
        /// <returns>An <see cref="IActionResult"/> containing the authentication token.</returns>
        /// <response code="200">Returns the authentication token.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            LoginValidator validator = new();
            await validator.ValidateAndThrowAsync(loginRequest);

            LoginResponse result = await _authService.LoginAsync(loginRequest);

            ApiResponse<LoginResponse> response = new()
            {
                Data = result,
                Message = "Connexion réussie"
            };

            return Ok(response);
        }

        /// <summary>
        /// Refresh the JWT token using a refresh token.
        /// </summary>
        /// <param name="tokenRequest">The token request containing the refresh token.</param>
        /// <returns>An <see cref="IActionResult"/> containing the new authentication token.</returns>
        /// <response code="200">Returns the new authentication token.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            TokenValidator validator = new();
            await validator.ValidateAndThrowAsync(tokenRequest);

            TokenResponse result = await _authService.RefreshTokenAsync(tokenRequest);

            ApiResponse<TokenResponse> response = new()
            {
                Data = result,
                Message = "JWT Token rafraîchi avec succès"
            };

            return Ok(response);
        }

        /// <summary>
        /// Create a new account user.
        /// </summary>
        /// <param name="registerRequest">The registration request containing user details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the registration.</returns>
        /// <response code="200">If the user was registered successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {

            RegisterValidator validator = new();

            await validator.ValidateAndThrowAsync(registerRequest);


            RegisterResponse result = await _authService.RegisterAsync(registerRequest);

            ApiResponse<RegisterResponse> response = new()
            {
                Data = result,
                Message = "Utilisateur enregistré avec succès"
            };

            return Ok(response);
        }
    }
}