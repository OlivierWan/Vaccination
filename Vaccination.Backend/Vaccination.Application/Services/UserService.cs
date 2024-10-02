using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Vaccination.Application.Dtos.User;
using Vaccination.Application.Exceptions;
using Vaccination.Application.Interfaces;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;

namespace Vaccination.Application.Services
{
    public class UserService(UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper) : IUserService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<User> _userManager = userManager;
        public async Task<bool> DeleteUserAsync(DeleteUserRequest deleteUserRequest)
        {
            User? user = await _userManager.FindByIdAsync(deleteUserRequest.UserId) ?? throw new NotFoundException("Utilisateur non trouvé");

            IdentityResult deleteResult = await _userManager.DeleteAsync(user);

            if (!deleteResult.Succeeded)
            {
                StringBuilder errorStringBuilder = new("Suppression de l'utilisateur a échoué car : ");
                foreach (IdentityError error in deleteResult.Errors)
                {
                    errorStringBuilder.Append(" # ").Append(error.Description);
                }

                throw new DatabaseException(errorStringBuilder.ToString());
            }

            return true;
        }

        public async Task<UserDetailsResponse> GetUserDetails(UserDetailsRequest userDetailsRequest)
        {
            User? user = await _userManager.FindByIdAsync(userDetailsRequest.UserId) ?? throw new NotFoundException("Utilisateur non trouvé");

            return _mapper.Map<UserDetailsResponse>(user);
        }

        public async Task<UpdateUserResponse> UpdateUserDetails(string userId, UpdateUserRequest updateUserRequest)
        {
            User? existingUser = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException("Utilisateur non trouvé");

            _mapper.Map(updateUserRequest, existingUser);

            IdentityResult updateResult = await _userManager.UpdateAsync(existingUser);

            if (!updateResult.Succeeded)
            {
                StringBuilder errorStringBuilder = new("Mise à jour de l'utilisateur a échoué car : ");
                foreach (IdentityError error in updateResult.Errors)
                {
                    errorStringBuilder.Append(" # ").Append(error.Description);
                }

                throw new DatabaseException(errorStringBuilder.ToString());
            }
            await _unitOfWork.SaveAsync();

            return _mapper.Map<UpdateUserResponse>(existingUser);
        }
    }
}