using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Core.Domain.Repositories;

namespace Api.Data.Repositories.Database.Dapper {
    public interface IAuditRepository : ICrudRepository<Audit, long> {
        Task<IEnumerable<Audit>> GetAllByAsync(
            int? year,
            int? month,
            long? userId
        );
    }
}