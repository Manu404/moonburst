using MoonBurst.ViewModel;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.Views
{
    public partial class DebugWindow
    {
        public DebugWindow(IMainViewModel context)
        {
            InitializeComponent();
            this.DataContext = context;
        }
    }
}
