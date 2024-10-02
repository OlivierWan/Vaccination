using Microsoft.AspNetCore.Identity;
using Vaccination.Application.Dtos.Reminder;
using Vaccination.Application.Exceptions;
using Vaccination.Application.Interfaces;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;

namespace Vaccination.Application.Services
{
    public class ReminderVaccinationService : IReminderVaccinationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ReminderVaccinationService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        public async Task<IEnumerable<ReminderVaccinationResponse>> GetUpcomingRemindersAsync(string userId)
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("Utilisateur non trouvé");
            }

            int userAgeInMonths = 0;

            // if user has a date of birth, we use it, else we estimate the age based on the vaccinations
            if (user.DateOfBirth.HasValue)
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                userAgeInMonths = CalculateAgeInMonths(user.DateOfBirth.Value, today);
            }
            else
            {
                IEnumerable<UserVaccination> userVaccinationsList = await _unitOfWork.UserVaccinations.GetUserVaccinationsByUserIdAsync(userId);
                if (userVaccinationsList.Any())
                {
                    userAgeInMonths = userVaccinationsList.Max(x => x.VaccineCalendar.MonthAge);
                }
            }

            IEnumerable<UserVaccination> userVaccinations = await _unitOfWork.UserVaccinations.GetUserVaccinationsByUserIdAsync(userId);
            List<UserVaccination> orderedUserVaccinations = userVaccinations.OrderBy(x => x.VaccinationDate).ToList();

            List<CalendarVaccination> calendarList = (await _unitOfWork.CalendarVaccinations.GetAllAsync())
                .Where(x => !orderedUserVaccinations.Exists(y => y.VaccineCalendarId == x.Id))
                .OrderBy(x => x.MonthAge)
                .ToList();

            List<ReminderVaccinationResponse> responses = new List<ReminderVaccinationResponse>();

            // next vaccine
            CalendarVaccination? nextVaccination = calendarList.Find(x => x.MonthAge >= userAgeInMonths);
            if (nextVaccination != null)
            {
                responses.Add(CreateReminderResponse(nextVaccination, "Prochain vaccin"));
            }

            // vaccines missed
            IEnumerable<CalendarVaccination> overdueVaccinations = calendarList.Where(x => x.MonthAge < userAgeInMonths);
            foreach (CalendarVaccination? overdueVaccination in overdueVaccinations)
            {
                responses.Add(CreateReminderResponse(overdueVaccination, "vaccin en retard"));
            }

            return responses.Distinct();
        }

        internal static int CalculateAgeInMonths(DateOnly birthDate, DateOnly currentDate)
        {
            int yearsDifference = currentDate.Year - birthDate.Year;
            int monthsDifference = currentDate.Month - birthDate.Month;

            if (monthsDifference < 0)
            {
                yearsDifference--;
                monthsDifference += 12;
            }

            return yearsDifference * 12 + monthsDifference;
        }

        private static ReminderVaccinationResponse CreateReminderResponse(CalendarVaccination vaccination, string message)
        {
            return new ReminderVaccinationResponse(
                Name: vaccination.Name,
                Description: vaccination.Description,
                MonthAge: vaccination.MonthAge,
                Message: message,
                CalendarVaccinationId: vaccination.Id
            );
        }
    }

}

