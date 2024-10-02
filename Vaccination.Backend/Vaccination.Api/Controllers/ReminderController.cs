using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vaccination.Application.Dtos;
using Vaccination.Application.Dtos.Reminder;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Shared;

namespace Vaccination.Api.Controllers
{
    /// <summary>
    /// Controller for managing vaccination reminders.
    /// </summary>
    public class ReminderController(IReminderVaccinationService reminderVaccinationService) : BaseController
    {
        private readonly IReminderVaccinationService _reminderVaccinationService = reminderVaccinationService;

        /// <summary>
        /// Retrieves the upcoming vaccination reminders for the authenticated user.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of upcoming reminders.</returns>
        /// <response code="200">Returns the list of upcoming reminders.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = RolesConstants.USER)]
        [Authorize(Roles = RolesConstants.READ)]
        public async Task<IActionResult> GetUpcomingRemindersAsync()
        {
            IEnumerable<ReminderVaccinationResponse> result = await _reminderVaccinationService.GetUpcomingRemindersAsync(GetUserId());

            ApiResponse<IEnumerable<ReminderVaccinationResponse>> response = new()
            {
                Data = result,
                Message = "Rappels à venir récupérés avec succès"
            };

            return Ok(response);
        }
    }
}
