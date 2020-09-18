using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Core.Data;
using Dapper;

namespace Api.Data.Repositories.Database.Dapper {
    public class AuditRepository :
        BaseRepository<IDbConnection>, IAuditRepository {
        public AuditRepository(IUnitOfWork<IDbConnection> source) :
            base(source) { }

        public async Task<Audit> GetByIdAsync(long id) {
            var result = await Connection.QueryAsync<Audit, User, Audit>(
                sql: GET_SQL_QUERY,
                param: new {
                    Id = id
                },
                map: (audit, user) => {
                    audit.User = user;
                    return audit;
                },
                splitOn: "id"
            );
            return result.SingleOrDefault();
        }


        public Task<IEnumerable<Audit>> GetAllAsync() =>
            Connection.QueryAsync<Audit, User, Audit>(
                sql: GET_ALL_SQL_QUERY,
                map: (audit, user) => {
                    audit.User = user;
                    return audit;
                },
                splitOn: "id"
            );

        public async Task<IEnumerable<Audit>> GetAllByAsync(
            int? year,
            int? month,
            long? userId
        ) {
            var sql = GET_ALL_SQL_QUERY + (year, month) switch {
                (null, null) => "",
                ({}, null) => " AND @Year = extract(year from date)",
                (null, {}) => " AND @Month = extract(month from date)",
                ({}, {}) =>
                    @" AND @Year * 100 + @Month = extract(year from date) 
                            * 100 + extract(month from date)"
            };

            sql += userId switch {
                null => "",
                {} => " AND @UserId = users.id"
            };

            return await Connection.QueryAsync<Audit, User, Audit>(
                sql: sql,
                param: new {
                    Year = year,
                    Month = month,
                    UserId = userId
                },
                map: (audit, user) => {
                    audit.User = user;
                    return audit;
                },
                splitOn: "id"
            );
        }


        public async Task SaveAsync(Audit entity) {
            var id = await Connection.QuerySingleAsync<long>(
                sql: INSERT_SQL_QUERY,
                param: new {
                    UserId = entity.User.Id,
                    Description = entity.Description,
                    Date = entity.Date,
                    Hours = entity.Hours
                }
            );
            entity.Id = id;
        }

        public Task UpdateAsync(Audit entity) =>
            Connection.ExecuteAsync(
                sql: UPDATE_SQL_QUERY,
                param: entity
            );

        public Task DeleteAsync(Audit entity) =>
            Connection.ExecuteAsync(
                sql: DELETE_SQL_QUERY,
                param: new {Id = entity.Id}
            );

        private const string GET_ALL_SQL_QUERY =
            "select * from audits JOIN users ON audits.user_id = users.id";

        private const string GET_SQL_QUERY =
            @"select * from Audits 
            JOIN users ON audits.user_id = users.id where audits.id=@Id";

        private const string INSERT_SQL_QUERY =
            @"insert into Audits (description, hours, date, user_id) 
            values (@Description, @Hours, @Date, @UserId) RETURNING id";

        private const string UPDATE_SQL_QUERY =
            @"update Audits 
            set description=@Description, hours=@Hours, date=@Date";

        private const string DELETE_SQL_QUERY =
            "DELETE from Audits WHERE Id = @Id";
    }
}