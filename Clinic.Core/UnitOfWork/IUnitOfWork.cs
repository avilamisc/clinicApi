using Clinic.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Clinic.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IBookingRepository BookingRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IClinicClinicianRepository ClinicClinicianRepository { get; }
        IDocumentRepository DocumentRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
