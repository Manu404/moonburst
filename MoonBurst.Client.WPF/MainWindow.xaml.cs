using System.Windows;
using System.Windows.Controls;
using MoonBurst.Core;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.View
{
    public class MainWindowFactory : IMainWindowFactory
    {
        private IMainViewModel mainViewModel;
        private IFactory<IMainView> viewFactory;

        public MainWindowFactory(IMainViewModel mainViewModel, IFactory<IMainView> viewFactory)
        {
            this.mainViewModel = mainViewModel;
            this.viewFactory = viewFactory;
        }
        public IMainViewHost Build()
        {
            return new MainWindow(mainViewModel, viewFactory);
        }
    }

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
