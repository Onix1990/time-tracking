using System.Data;
using Api.Common.Settings;
using Core.Data;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Api.Data.Database {
    public class DapperDatabase : IDatabase<IDbConnection> {
        private readonly DatabaseSettings databaseSettings;

        public DapperDatabase(
            IOptionsMonitor<DatabaseSettings> databaseSettingsOptionsMonitor) {
            databaseSettings = databaseSettingsOptionsMonitor.CurrentValue;
            DefineMapping();
        }

        public IDbConnection CreateSource() {
            var connection = new NpgsqlConnection(
                new NpgsqlConnectionStringBuilder {
                    Host = databaseSettings.Host,
                    Database = databaseSettings.Database,
                    Username = databaseSettings.Username,
                    Password = databaseSettings.Password
                }.ConnectionString
            );
            connection.Open();

            return connection;
        }

        private static void DefineMapping() {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}