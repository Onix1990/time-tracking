using System.Threading.Tasks;
using Api.Domain.Entities;
using Core.Domain.Repositories;

namespace Api.Data.Repositories.Database.Dapper {
    public interface IUserRepository : ICrudRepository<User, long> {
        Task<User> GetByEmailAsync(string email);
    }
}