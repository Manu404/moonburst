using MoonBurst.Core;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst
{
    /// <summary>
    /// Interaction logic for WindowMainViewHost.xaml
    /// </summary>
    public partial class WindowMainViewHost : IMainViewHost
    {
        private IMainView View;
        public WindowMainViewHost()
        {
            InitializeComponent();
        }

        public WindowMainViewHost(IMainViewModel vm, IFactory<IMainView> view)
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
