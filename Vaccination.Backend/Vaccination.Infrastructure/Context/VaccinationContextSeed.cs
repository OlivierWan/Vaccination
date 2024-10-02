using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaccination.Domain.Entities;

namespace Vaccination.Infrastructure.Context
{
    public static class VaccinationContextSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "USER", NormalizedName = "USER", ConcurrencyStamp = Guid.NewGuid().ToString() },
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "OWNER", NormalizedName = "OWNER", ConcurrencyStamp = Guid.NewGuid().ToString() },
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "ADMIN", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() },
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "READ", NormalizedName = "READ", ConcurrencyStamp = Guid.NewGuid().ToString() },
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "WRITE", NormalizedName = "WRITE", ConcurrencyStamp = Guid.NewGuid().ToString() },
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "DELETE", NormalizedName = "DELETE", ConcurrencyStamp = Guid.NewGuid().ToString() }
        );

            modelBuilder.Entity<CalendarVaccination>().HasData(
                new CalendarVaccination
                {
                    Id = Guid.NewGuid(),
                    Name = "RSV (virus respiratoire syncytial)",
                    Description = "administration d’un traitement\r\npréventif (produit d’immunisation passive) qui protège contre la\r\nbronchiolite, de préférence avant la sortie de la maternité, en période\r\nde haute circulation du virus, de septembre à février.",
                    MonthAge = 0,
                    MonthDelay = 6,
                    CreatedBy = Guid.Empty,
                    CreatedOnUtc = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CalendarVaccination
                {
                    Id = Guid.NewGuid(),
                    Name = "1ère dose du vaccin combiné (D, T, aP, Hib, IPV, Hep B)",
                    Description = "protège\r\ncontre :\r\n- la diphtérie,\r\n- le tétanos,\r\n- la coqueluche,\r\n- les infections invasives à Haemophilus Influenzae de type b\r\n(méningite, épiglottite et arthrite),\r\n- la poliomyélite,\r\n- l’hépatite B.\r\nRotavirus (1ère dose) : vaccination contre la gastro-entérite à rotavirus.\r\nPneumocoques (1ère dose) : vaccination contre les infections\r\ninvasives à pneumocoques.",
                    MonthAge = 2,
                    MonthDelay = 0,
                    CreatedBy = Guid.Empty,
                    CreatedOnUtc = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CalendarVaccination
                {
                    Id = Guid.NewGuid(),
                    Name = "Rotavirus (2ème dose)",
                    Description = "vaccination contre la gastro-entérite à\r\nrotavirus.\r\nMéningocoque B (1ère dose) : vaccination contre les infections\r\ninvasives à méningocoque B.",
                    MonthAge = 3,
                    MonthDelay = 0,
                    CreatedBy = Guid.Empty,
                    CreatedOnUtc = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CalendarVaccination
                {
                    Id = Guid.NewGuid(),
                    Name = "2ème dose du vaccin combiné (D, T, aP, Hib, IPV, Hep B)",
                    Description = "2ème dose du vaccin combiné (D, T, aP, Hib, IPV, Hep B) qui\r\nprotège contre :\r\n- la diphtérie,\r\n- le tétanos,\r\n- la coqueluche,\r\n- les infections invasives à Haemophilus Influenzae de type b\r\n(méningite, épiglottite et arthrite),\r\n- la poliomyélite,\r\n- l’hépatite B.\r\nPneumocoques (2ème dose) : vaccination contre les infections\r\ninvasives à pneumocoques.\r\nRotavirus (3ème dose) : vaccination contre la gastro-entérite à\r\nrotavirus.",
                    MonthAge = 4,
                    MonthDelay = 0,
                    CreatedBy = Guid.Empty,
                    CreatedOnUtc = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CalendarVaccination
                {
                    Id = Guid.NewGuid(),
                    Name = "Méningocoque B (2ème dose)",
                    Description = "vaccination contre les infections\r\ninvasives à méningocoque B.",
                    MonthAge = 5,
                    MonthDelay = 0,
                    CreatedBy = Guid.Empty,
                    CreatedOnUtc = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CalendarVaccination
                {
                    Id = Guid.NewGuid(),
                    Name = "3ème dose du vaccin combiné (D, T, aP, Hib, IPV, Hep B)",
                    Description = "protège contre :\r\n- la diphtérie,\r\n- le tétanos,\r\n- la coqueluche,\r\n- les infections invasives à Haemophilus Influenzae de type b\r\n(méningite, épiglottite et arthrite),\r\n- la poliomyélite,\r\n- l’hépatite B.\r\nPneumocoques (3ème dose) : vaccination contre les infections\r\ninvasives à pneumocoques.",
                    MonthAge = 11,
                    MonthDelay = 0,
                    CreatedBy = Guid.Empty,
                    CreatedOnUtc = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CalendarVaccination
                {
                    Id = Guid.NewGuid(),
                    Name = "1ère dose du vaccin combiné (RORV)",
                    Description = "protège contre :\r\n- la rougeole,\r\n- les oreillons,\r\n- la rubéole,\r\n- la varicelle.\r\nMéningocoque B (3ème dose) : vaccination contre les infections\r\ninvasives à méningocoque B.",
                    MonthAge = 12,
                    MonthDelay = 0,
                    CreatedBy = Guid.Empty,
                    CreatedOnUtc = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CalendarVaccination
                {
                    Id = Guid.NewGuid(),
                    Name = "Méningocoques ACWY (1ère dose)",
                    Description = "vaccination contre les infections\r\ninvasives à méningocoques A, C, W et Y",
                    MonthAge = 13,
                    MonthDelay = 0,
                    CreatedBy = Guid.Empty,
                    CreatedOnUtc = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}