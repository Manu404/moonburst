using System.Windows.Controls;

namespace MoonBurst.Api.Client
{
    public interface IMainViewHost
    {
        void Start();
        double Width { get; set; }
        double Height { get; set; }
        UserControl RootControl { get; }
    }
}