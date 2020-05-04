using MoonBurst.Api.Client;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst
{
    /// <summary>
    /// Interaction logic for WindowMainViewHost.xaml
    /// </summary>
    public sealed partial class WindowMainViewHost : IMainViewHost
    {
        public WindowMainViewHost()
        {
            InitializeComponent();
        }

        public WindowMainViewHost(IMainViewModel vm, IFactory<IMainView> view)
        {
            InitializeComponent();
            this.AddChild(view.Build());
            this.DataContext = vm;
        }

        public void Start()
        {
            this.Show();
        }
    }
}
