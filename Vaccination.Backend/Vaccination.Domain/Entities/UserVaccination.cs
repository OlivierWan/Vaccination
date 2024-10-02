using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vaccination.Domain.Shared;

namespace Vaccination.Domain.Entities
{
    public class UserVaccination : IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public required string UserId { get; set; }

        [Required]
        [ForeignKey("VaccineCalendar")]
        public Guid VaccineCalendarId { get; set; }

        [Required]
        public DateOnly VaccinationDate { get; set; }

        public string? Description { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }

        public required virtual User User { get; set; }
        public required virtual CalendarVaccination VaccineCalendar { get; set; }

    }
}