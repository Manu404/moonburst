namespace MoonBurst.Api.Client
{
    public interface ILauncher
    {
        void Initialize();
        void Run();
        IMainViewHost Host { get; }
    }
}