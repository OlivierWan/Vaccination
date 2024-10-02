using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Vaccination.Domain.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(150)]
        [Required]
        public required string FirstName { get; set; }

        [MaxLength(150)]
        [Required]
        public required string LastName { get; set; } 

        public DateOnly? DateOfBirth { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? Nationality { get; set; }

        public string? Address { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(13)]
        public string? SocialSecurityNumber { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ICollection<UserVaccination> UserVaccinations { get; set; } = [];

    }
}