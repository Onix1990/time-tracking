using System.Data;
using Core.Data;

namespace Api.Data.Database {
    public class DapperUnitOfWork : UnitOfWork<IDbConnection, IDbTransaction> {
        public DapperUnitOfWork(IDatabase<IDbConnection> database) :
            base(database) { }

        protected override void CommitTransaction() {
            Transaction.Commit();
            Source.Close();
        }

        protected override void RollbackTransaction() {
            Transaction.Rollback();
            Source.Close();
        }

        protected override void BeginTransaction(
            IsolationLevel transactionIsolationLevel) {
            Source = Database.CreateSource();
            Transaction = Source.BeginTransaction(transactionIsolationLevel);
        }
    }
}