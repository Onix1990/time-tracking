namespace Core.Data {
    public interface IDatabase<out TSource> where TSource : class {
        TSource CreateSource();
    }
}