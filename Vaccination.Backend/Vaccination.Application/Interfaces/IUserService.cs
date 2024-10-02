using Vaccination.Application.Dtos.User;

namespace Vaccination.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> DeleteUserAsync(DeleteUserRequest deleteUserRequest);

        Task<UserDetailsResponse> GetUserDetails(UserDetailsRequest userDetailsRequest);

        Task<UpdateUserResponse> UpdateUserDetails(string userId, UpdateUserRequest updateUserRequest);
    }
}