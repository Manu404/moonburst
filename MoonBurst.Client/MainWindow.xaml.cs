using MoonBurst.Core;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainViewHost
    {
        private IMainView View;
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(IMainViewModel vm, IFactory<IMainView> view)
        {
            InitializeComponent();
            View = view.Build();
            this.AddChild(View);
            this.DataContext = vm;
        }


        public void Start()
        {
            this.Show();
        }
    }
}
