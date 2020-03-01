using MoonBurst.Model;

namespace MoonBurst.Core
{
    public interface ISerializer<T>
    {
        void Save(T source, string path);
        void Load(string path, T target);
        void LoadDefault(T target);
        void SaveDefault(T source);
    }

    public interface ISerializer<T, Y> : ISerializer<T> where Y : new()
    {
    }
}