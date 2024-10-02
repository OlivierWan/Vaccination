using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vaccination.Application.Dtos;
using Vaccination.Application.Dtos.User;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Shared;
using Vaccination.Application.Validators.User;

namespace Vaccination.Api.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    public class UserController(IUserService userService) : BaseController
    {
        private readonly IUserService userService = userService;

        /// <summary>
        /// Retrieves the details of the authenticated user.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the user details.</returns>
        /// <response code="200">Returns the user details.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [Authorize(Roles = RolesConstants.USER)]
        [Authorize(Roles = RolesConstants.READ)]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            UserDetailsRequest userDetailsRequest = new(GetUserId());

            GetUserValidator validator = new();
            await validator.ValidateAndThrowAsync(userDetailsRequest);

            UserDetailsResponse userDetails = await userService.GetUserDetails(userDetailsRequest);

            ApiResponse<UserDetailsResponse> response = new()
            {
                Data = userDetails,
                Message = "Détails de l'utilisateur récupérés avec succès"
            };

            return Ok(response);
        }

        /// <summary>
        /// Update the details of the authenticated user.
        /// </summary>
        /// <param name="updateUserRequest">The user details to update.</param>
        /// <returns>An <see cref="IActionResult"/> containing the updated user details.</returns>
        /// <response code="200">Returns the updated user details.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [Authorize(Roles = RolesConstants.USER)]
        [Authorize(Roles = RolesConstants.WRITE)]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updateUserRequest)
        {
            UpdateUserValidator validator = new();
            await validator.ValidateAndThrowAsync(updateUserRequest);

            UpdateUserResponse updateUserResult = await userService.UpdateUserDetails(GetUserId(), updateUserRequest);

            ApiResponse<UpdateUserResponse> response = new()
            {
                Data = updateUserResult,
                Message = "Utilisateur mis à jour avec succès"
            };

            return Ok(response);
        }

        /// <summary>
        /// Delete the authenticated user's account.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the deletion.</returns>
        /// <response code="200">If the user account was successfully deleted.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [Authorize(Roles = RolesConstants.USER)]
        [Authorize(Roles = RolesConstants.DELETE)]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            DeleteUserRequest deleteUserRequest = new(GetUserId());

            bool deleteAccountResult = await userService.DeleteUserAsync(deleteUserRequest);

            ApiResponse<DeleteUserResponse> response = new()
            {
                Data = new DeleteUserResponse(IsDeleted: deleteAccountResult),
                Message = "Utilisateur supprimé avec succès"
            };

            return Ok(response);
        }
    }
}