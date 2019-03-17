using Clinic.Core.UnitOfWork;
using Clinic.Core.Repositories;
using Clinic.Data.Context;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Repositories;
using System.Threading.Tasks;

namespace Clinic.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicDb _context;
        private readonly IDataMapper _mapper;

        public UnitOfWork(
            IDataMapper mapper,
            ClinicDb context)
        {
            _mapper = mapper;
            _context = context;

            UserRepository = new UserRepository(mapper, context);
            BookingRepository = new BookingRepository(mapper, context);
            RefreshTokenRepository = new RefreshTokenRepository(mapper, context);
            ClinicClinicianRepository = new ClinicClinicianRepository(context);
            DocumentRepository = new DocumentRepository(context);
            ClinicRepository = new ClinicRepository(mapper, context);
            ClinicianRepository = new ClinicianRepository(mapper, context);
            PatientRepository = new PatientRepository(context);
        }

        public IUserRepository UserRepository { get; private set; }

        public IBookingRepository BookingRepository { get; private set; }

        public IRefreshTokenRepository RefreshTokenRepository { get; private set; }

        public IClinicClinicianRepository ClinicClinicianRepository { get; private set; }

        public IDocumentRepository DocumentRepository { get; private set; }

        public IClinicRepository ClinicRepository { get; private set; }

        public IClinicianRepository ClinicianRepository { get; private set; }

        public IPatientRepository PatientRepository { get; private set; }

        public INotificationRepository NotificationRepository { get; private set; }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
