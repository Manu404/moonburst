using MahApps.Metro.Controls;
using MoonBurst.ViewModel;

namespace MoonBurst.Views
{
    public partial class DebugWindow : MetroWindow
    {
        public DebugWindow(IMainViewModel context)
        {
            InitializeComponent();
            this.DataContext = context;
        }
    }
}
