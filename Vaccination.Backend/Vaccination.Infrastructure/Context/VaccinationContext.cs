using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vaccination.Domain.Entities;

namespace Vaccination.Infrastructure.Context
{
    public class VaccinationContext(DbContextOptions<VaccinationContext> options) : IdentityDbContext<User>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.Property(u => u.Email).HasMaxLength(128);
                b.Property(u => u.NormalizedEmail).HasMaxLength(128);
                b.HasMany(u => u.UserVaccinations)
                 .WithOne(uv => uv.User)
                 .HasForeignKey(uv => uv.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserVaccination>(b =>
            {
                b.HasQueryFilter(x => !x.IsDeleted);
                b.HasOne(uv => uv.User)
                 .WithMany(u => u.UserVaccinations)
                 .HasForeignKey(uv => uv.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(uv => uv.VaccineCalendar)
                 .WithOne()
                 .HasForeignKey<UserVaccination>(uv => uv.VaccineCalendarId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<CalendarVaccination>(b =>
            {
                b.HasQueryFilter(x => !x.IsDeleted);
            });

            VaccinationContextSeed.Seed(builder);

        }

        public DbSet<UserVaccination> UserVaccinations { get; set; }
        public DbSet<CalendarVaccination> CalendarVaccinations { get; set; }
    }
}