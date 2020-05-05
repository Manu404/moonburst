using System.Windows.Controls;
using System.Windows.Media;
using MoonBurst.Api.Client;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.Vst
{
    /// <summary>
    /// Interaction logic for UserControlMainViewHost.xaml
    /// </summary>
    public sealed partial class UserControlMainViewHost : IMainViewHost
    {
        public UserControlMainViewHost()
        {
            InitializeComponent();
        }

        public UserControlMainViewHost(IMainViewModel vm, IFactory<IMainView> view)
        {
            InitializeComponent();
            this.AddChild(view.Build());
            this.DataContext = vm;
        }

        public void Start()
        {
            
        }

        public double Width
        {
            get => base.Width;
            set => base.Width = value;
        }

        public double Height
        {
            get => base.Height;
            set => base.Height = value;
        }

        public UserControl RootControl => this;
    }
}
