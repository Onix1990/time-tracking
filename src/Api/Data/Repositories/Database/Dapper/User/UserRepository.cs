using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Core.Data;
using Dapper;

namespace Api.Data.Repositories.Database.Dapper {
    public class UserRepository :
        BaseRepository<IDbConnection>, IUserRepository {
        public UserRepository(IUnitOfWork<IDbConnection> source) :
            base(source) { }

        public Task<User> GetByIdAsync(long id) =>
            Connection.QueryFirstOrDefaultAsync<User>(
                sql: GET_BY_ID_SQL_QUERY,
                param: new {
                    Id = id
                }
            );

        public Task<IEnumerable<User>> GetAllAsync() =>
            Connection.QueryAsync<User>(GET_ALL_SQL_QUERY);


        public async Task SaveAsync(User entity) {
            var id = await Connection.QuerySingleAsync<long>(
                sql: INSERT_SQL_QUERY,
                param: entity
            );
            entity.Id = id;
        }

        public Task UpdateAsync(User entity) =>
            Connection.ExecuteAsync(
                sql: UPDATE_SQL_QUERY,
                param: entity
            );

        public Task DeleteAsync(User entity) =>
            Connection.ExecuteAsync(
                sql: DELETE_SQL_QUERY,
                param: new {Id = entity.Id}
            );

        public Task<User> GetByEmailAsync(string email) =>
            Connection.QueryFirstOrDefaultAsync<User>(
                sql: GET_BY_EMAIL_SQL_QUERY,
                param: new {
                    Email = email
                }
            );

        private const string GET_ALL_SQL_QUERY = "select * from users";

        private const string GET_BY_ID_SQL_QUERY =
            "select * from users where id=@Id";

        private const string INSERT_SQL_QUERY =
            @"insert into users (email, first_name, last_name, patronymic) 
            values (@Email, @FirstName, @LastName, @Patronymic) RETURNING id";

        private const string UPDATE_SQL_QUERY =
            @"update users 
            set email=@Email, first_name=@FirstName, 
            last_name=@LastName, patronymic=@Patronymic";

        private const string DELETE_SQL_QUERY =
            "DELETE from users WHERE Id = @Id";


        private const string GET_BY_EMAIL_SQL_QUERY =
            "select * from users where email=@Email";
    }
}