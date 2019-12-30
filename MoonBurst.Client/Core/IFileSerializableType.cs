namespace MoonBurst.ViewModel
{
    public interface IFileSerializableData
    {

    }

    public interface IFileSerializableType
    {
        string Path { get; set; }
        string Default { get; }
    }


    public interface IFileSerializer<T>
    {
        string Path { get; set; }
        string Default { get; }
        T GetData();
        void SaveData(string path);
    }
}