using System;
using System.Data;
using System.Runtime.CompilerServices;
using Serilog;

namespace Core.Data {
    public abstract class UnitOfWork<TSource, TTransaction> :
        IUnitOfWork<TSource>
        where TSource : class, IDisposable
        where TTransaction : IDisposable {
        public TSource Source { get; protected set; }
        protected TTransaction Transaction { get; set; }
        protected IDatabase<TSource> Database { get; }

        public UnitOfWorkState State { get; private set; } =
            UnitOfWorkState.Ready;

        protected UnitOfWork(IDatabase<TSource> database) {
            Database = database;
        }

        protected abstract void BeginTransaction(
            IsolationLevel transactionIsolationLevel);

        protected abstract void CommitTransaction();
        protected abstract void RollbackTransaction();

        public void Begin(
            IsolationLevel transactionIsolationLevel =
                IsolationLevel.ReadCommitted) {
            ThrowIfInvalidState(UnitOfWorkState.Ready);
            Log.Debug(BEGIN_INFO_MESSAGE);
            BeginTransaction(transactionIsolationLevel);
            State = UnitOfWorkState.Begun;
        }

        public virtual void Commit() {
            ThrowIfInvalidState(UnitOfWorkState.Begun);

            try {
                CommitTransaction();
                State = UnitOfWorkState.Committed;
            }
            catch (Exception exception) {
                Log.Error(
                    exception: exception,
                    messageTemplate: COMMIT_ERROR_MESSAGE
                );
                RollbackTransaction();
                throw;
            }
        }

        public virtual void Rollback() {
            ThrowIfInvalidState(UnitOfWorkState.Begun);

            try {
                RollbackTransaction();
                State = UnitOfWorkState.Rollbacked;
            }
            catch (Exception exception) {
                Log.Error(
                    exception: exception,
                    messageTemplate: ROLLBACK_ERROR_MESSAGE
                );
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfInvalidState(UnitOfWorkState expectedState) {
            if (expectedState != State) {
                throw new InvalidOperationException(
                    $"The unit of work is required " +
                    $"to be in {expectedState} state for this action. " +
                    $"Current state is {State}"
                );
            }
        }

        public void Dispose() {
            Transaction?.Dispose();
            Source?.Dispose();
        }

        private const string BEGIN_INFO_MESSAGE =
            "Beginning a new unit of work";

        private const string ROLLBACK_ERROR_MESSAGE =
            "An error has occured during rollbacking transaction";

        private const string COMMIT_ERROR_MESSAGE =
            "An error has occured during committing transaction";
    }
}