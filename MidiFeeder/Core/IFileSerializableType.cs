namespace MoonBurst.ViewModel
{
    public interface IFileSerializableData
    {

    }

    public interface IFileSerializableType<T> where T : IFileSerializableData
    {
        string Path { get; set; }
        string Default { get; }
        T GetData();
        IFileSerializableType<T> CreateFromData(T data, string path);
    }
}