using System;
using System.Windows.Controls;
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

        public UserControl RootControl => throw new NotImplementedException();
    }
}
