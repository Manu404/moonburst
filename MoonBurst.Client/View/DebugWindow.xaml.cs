using MoonBurst.ViewModel.Interface;

namespace MoonBurst.View
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
