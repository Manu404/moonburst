namespace MoonBurst.Api.Client
{
    public interface IApp
    {
        void Initialize();
        void Run();
        IMainViewHost Host { get; }
    }
}