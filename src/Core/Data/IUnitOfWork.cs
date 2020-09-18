using System;
using System.Data;

namespace Core.Data {
    public interface IUnitOfWork<out TSource> :
        IDisposable
        where TSource : class {
        TSource Source { get; }

        UnitOfWorkState State { get; }

        void Begin(
            IsolationLevel transactionIsolationLevel =
                IsolationLevel.ReadCommitted
        );

        void Commit();

        void Rollback();
    }
}