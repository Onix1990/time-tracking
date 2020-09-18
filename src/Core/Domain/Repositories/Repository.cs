namespace Core.Domain.Repositories {
    public abstract class Repository<TSource> where TSource : class {
        protected TSource Source { get; }

        protected Repository(TSource source) {
            Source = source;
        }
    }
}