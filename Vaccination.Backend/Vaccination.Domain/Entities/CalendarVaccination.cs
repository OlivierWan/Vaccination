using System.ComponentModel.DataAnnotations;
using Vaccination.Domain.Shared;

namespace Vaccination.Domain.Entities
{
    public class CalendarVaccination : IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required int MonthAge { get; set; }

        //Months after the required age to be vaccinated
        [Required]
        public required int MonthDelay { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}