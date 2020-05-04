using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;

namespace MoonBurst
{
    public interface ILauncher
    {
        void Initialize();
        void Run();
        IMainViewHost Host { get; }
    }
}