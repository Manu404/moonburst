using MoonBurst.ViewModel;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.Views
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(IMainViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
