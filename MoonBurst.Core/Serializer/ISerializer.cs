namespace MoonBurst.Core.Serializer
{
    public interface ISerializer<in T>
    {
        void Save(T source, string path);
        void Load(string path, T target);
        void LoadDefault(T target);
        void SaveDefault(T source);
    }

    public interface ISerializer<in T, TY> : ISerializer<T> where TY : new()
    {
    }
}