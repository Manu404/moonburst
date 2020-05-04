namespace MoonBurst.Api.Serializer
{
    public interface IDataExtractor<in T>
    {
        object ExtractData(T source);
        void ApplyData(object source, T target);
    }

    public interface IDataExtractor<in T, TY> where TY : new()
    {
        TY ExtractData(T source);
        void ApplyData(TY source, T target);
    }
}