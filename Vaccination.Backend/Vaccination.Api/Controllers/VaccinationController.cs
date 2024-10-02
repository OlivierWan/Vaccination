using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Vaccination.Application.Dtos;
using Vaccination.Application.Dtos.Vaccination;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Shared;
using Vaccination.Application.Validators.Vaccination;
using Vaccination.Domain.Shared;

namespace Vaccination.Api.Controllers
{
    /// <summary>
    /// Controller for managing user vaccinations.
    /// </summary>
    public class VaccinationController(IUserVaccinationService userVaccinationService) : BaseController
    {
        private readonly IUserVaccinationService _userVaccinationService = userVaccinationService;

        /// <summary>
        /// Create a new vaccination record for the authenticated user.
        /// </summary>
        /// <param name="createUserVaccinationRequest">The vaccination details to create.</param>
        /// <returns>An <see cref="IActionResult"/> containing the created vaccination details.</returns>
        /// <response code="201">Returns the created vaccination details.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles = RolesConstants.USER)]
        [Authorize(Roles = RolesConstants.WRITE)]
        public async Task<IActionResult> Create([FromBody] CreateUserVaccinationRequest createUserVaccinationRequest)
        {
            CreateUserVaccinationValidator validator = new();
            await validator.ValidateAndThrowAsync(createUserVaccinationRequest);

            CreatedUserVaccinationResponse result = await _userVaccinationService.CreateVaccinationAsync(createUserVaccinationRequest, GetUserId());

            ApiResponse<CreatedUserVaccinationResponse> response = new()
            {
                Data = result,
                Message = "Vaccination créée avec succès"
            };

            return CreatedAtAction(nameof(Create), response);
        }

        /// <summary>
        /// Delete a vaccination record by its ID.
        /// </summary>
        /// <param name="id">The ID of the vaccination record to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the deletion.</returns>
        /// <response code="200">If the vaccination record was successfully deleted.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = RolesConstants.USER)]
        [Authorize(Roles = RolesConstants.DELETE)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            bool result = await _userVaccinationService.DeleteVaccinationAsync(id);

            ApiResponse<bool> response = new()
            {
                Data = result,
                Message = "Vaccination supprimée avec succès"
            };

            return Ok(response);
        }

        /// <summary>
        /// Update a vaccination record by its ID.
        /// </summary>
        /// <param name="id">The ID of the vaccination record to update.</param>
        /// <param name="updateUserVaccinationRequest">The updated vaccination details.</param>
        /// <returns>An <see cref="IActionResult"/> containing the updated vaccination details.</returns>
        /// <response code="200">Returns the updated vaccination details.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = RolesConstants.USER)]
        [Authorize(Roles = RolesConstants.WRITE)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserVaccinationRequest updateUserVaccinationRequest)
        {
            UpdateUserVaccinationValidator validator = new();
            await validator.ValidateAndThrowAsync(updateUserVaccinationRequest);

            UpdateUserVaccinationResponse result = await _userVaccinationService.UpdateVaccinationAsync(id, updateUserVaccinationRequest);

            ApiResponse<UpdateUserVaccinationResponse> response = new()
            {
                Data = result,
                Message = "Vaccination mise à jour avec succès"
            };

            return Ok(response);
        }

        /// <summary>
        /// Retrieves all vaccination records for the authenticated user with optional filtering.
        /// </summary>
        /// <param name="getFilteredUserVaccinationRequest">The filtering criteria.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of vaccination records.</returns>
        /// <response code="200">Returns the list of vaccination records.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = RolesConstants.USER)]
        [Authorize(Roles = RolesConstants.READ)]
        public async Task<IActionResult> GetAll([FromQuery] GetFilteredUserVaccinationRequest getFilteredUserVaccinationRequest)
        {

            GetUserVaccinationValidator validator = new();
            await validator.ValidateAndThrowAsync(getFilteredUserVaccinationRequest);

            PagedList<UserVaccinationResponse> result = await _userVaccinationService.GetAllUserVaccinationsAsync(getFilteredUserVaccinationRequest, GetUserId());

            ApiResponse<PagedList<UserVaccinationResponse>> response = new()
            {
                Data = result,
                Message = "Vaccinations récupérées avec succès"
            };

            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(response);
        }
    }
}