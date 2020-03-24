namespace MoonBurst.Core
{
    public interface IDataExtractor<T>
    {
        object ExtractData(T source);
        void ApplyData(object source, T target);
    }

    public interface IDataExtractor<T, Y> where Y : new()
    {
        Y ExtractData(T source);
        void ApplyData(Y source, T target);
    }
}