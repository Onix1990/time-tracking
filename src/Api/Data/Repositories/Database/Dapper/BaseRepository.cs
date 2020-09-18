using Core.Data;
using Core.Domain.Repositories;

namespace Api.Data.Repositories.Database.Dapper {
    public abstract class BaseRepository<TSource> :
        Repository<IUnitOfWork<TSource>>
        where TSource : class {
        protected TSource Connection => Source.Source;

        protected BaseRepository(IUnitOfWork<TSource> source) : base(source) { }
    }
}