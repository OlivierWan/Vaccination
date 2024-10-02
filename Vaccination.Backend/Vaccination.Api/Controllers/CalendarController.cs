using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vaccination.Application.Dtos;
using Vaccination.Application.Dtos.Calendar;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Services;
using Vaccination.Application.Shared;

namespace Vaccination.Api.Controllers
{

    public class CalendarController(ICalendarVaccinationService calendarVaccinationService) : BaseController
    {
        private readonly ICalendarVaccinationService _calendarVaccinationService = calendarVaccinationService;

        /// <summary>
        /// Retrieves the complete calendar vaccination or the next vaccinations for the authenticated user.
        /// </summary>
        /// <param name="next">If true, retrieves the next vaccinations for the authenticated user.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of calendar vaccinations.</returns>
        /// <response code="200">Returns the list of calendar vaccinations.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = RolesConstants.USER)]
        [Authorize(Roles = RolesConstants.READ)]
        public async Task<IActionResult> GetAll([FromQuery] bool next = false)
        {
            IEnumerable<CalendarVaccinationResponse> result;

            if (next)
            {
                result = await _calendarVaccinationService.GetNextVaccines(GetUserId());
            }
            else
            {
                result = await _calendarVaccinationService.GetAllAsync();
            }

            ApiResponse<IEnumerable<CalendarVaccinationResponse>> response = new()
            {
                Data = result,
                Message = "Calendrier des vaccinations récupéré avec succès"
            };

            return Ok(response);
        }
    }
}