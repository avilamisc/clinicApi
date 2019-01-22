using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using System.Data.Entity;
using System.Linq;

namespace Clinic.Data.Common
{
    public static class CommonExtensions
    {
        public static IQueryable<Booking> BookingInclude(this IQueryable<Booking> query)
        {
            return query
                .OrderByDescending(b => b.Id)
                .Include(b => b.ClinicClinician.Clinic)
                .Include(b => b.Documents);
        }

        public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> query, PagingDto pagingDto)
        {
            return query
                .Skip(pagingDto.PageNumber * pagingDto.PageSize)
                .Take(pagingDto.PageSize);
        }
    }
}
