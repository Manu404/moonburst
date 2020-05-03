using MoonBurst.ViewModel.Interface;

namespace MoonBurst.View
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
