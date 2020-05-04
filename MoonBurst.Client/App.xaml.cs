using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using MoonBurst.Core;
using MoonBurst.Core.Helper;
using MoonBurst.Core.Serializer;
using MoonBurst.Helper;
using MoonBurst.View;
using MoonBurst.ViewModel;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : ILauncher
    {
        public IMainViewHost Host { get; private set; }

        private IMainWindowFactory _mainWindowFactory;

        public App(IMainWindowFactory mainWindowFactory)
        {
            this._mainWindowFactory = mainWindowFactory;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Color primaryColor = SwatchHelper.Lookup[MaterialDesignColor.DeepPurple200];
            Color accentColor = SwatchHelper.Lookup[MaterialDesignColor.Blue200];
            ITheme theme = Theme.Create(new MaterialDesignDarkTheme(), primaryColor, accentColor);
            Resources.SetTheme(theme);
            Host = _mainWindowFactory.Build();
            Host.Start();
            base.OnStartup(e);
        }

        public void Launch()
        {
            SplashScreen splashScreen = new SplashScreen(Assembly.GetAssembly(GetType()),"img/smallsplash.png");
            splashScreen.Show(true);
            this.InitializeComponent();
            Run();
        }

    }
}
