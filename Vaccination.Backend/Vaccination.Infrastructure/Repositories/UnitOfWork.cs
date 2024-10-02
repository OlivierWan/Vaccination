using Vaccination.Domain.Interfaces;
using Vaccination.Infrastructure.Context;
using Vaccination.Infrastructure.Exceptions;

namespace Vaccination.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly VaccinationContext context;
        private bool disposed = false;

        public UnitOfWork(VaccinationContext context)
        {
            this.context = context;
            CalendarVaccinations = new CalendarVaccinationRepository(this.context);
            UserVaccinations = new UserVaccinationRepository(this.context);
        }

        public ICalendarVaccinationRepository CalendarVaccinations { get; private set; }
        public IUserVaccinationRepository UserVaccinations { get; private set; }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue de la sauvegarde des changements dans la base de données");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }
    }
}