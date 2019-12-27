using MahApps.Metro.Controls;
using MoonBurst.ViewModel;

namespace MoonBurst.Views
{
    /// <summary>
    /// Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : MetroWindow
    {
        public DebugWindow(MainViewModel context)
        {
            InitializeComponent();
            this.DataContext = context;
        }
    }
}
