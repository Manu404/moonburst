using MoonBurst.Api.Client;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.Vst
{
    /// <summary>
    /// Interaction logic for VstMainViewHost.xaml
    /// </summary>
    public sealed partial class VstMainViewHost : IMainViewHost
    {
        public VstMainViewHost()
        {
            InitializeComponent();
        }

        public VstMainViewHost(IMainViewModel vm, IFactory<IMainView> view)
        {
            InitializeComponent();
            this.AddChild(view.Build());
            this.DataContext = vm;
        }

        public void Start()
        {
            
        }
    }
}
