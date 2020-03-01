namespace MoonBurst.Core
{
    public interface IExtractor<T>
    {
        object ExtractData(T source);
        void ApplyData(object source, T target);
    }

    public interface IExtractor<T, Y> where Y : new()
    {
        Y ExtractData(T source);
        void ApplyData(Y source, T target);
    }
}