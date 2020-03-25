namespace MoonBurst.Core.Serializer
{
    public interface IDataExtractor<T>
    {
        object ExtractData(T source);
        void ApplyData(object source, T target);
    }

    public interface IDataExtractor<T, TY> where TY : new()
    {
        TY ExtractData(T source);
        void ApplyData(TY source, T target);
    }
}