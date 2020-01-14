namespace MoonBurst.Model
{
    public interface IFileSerializableType
    {
        string Path { get; set; }
        string Default { get; }
    }
}