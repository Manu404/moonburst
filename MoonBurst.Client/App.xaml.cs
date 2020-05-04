using System.CodeDom.Compiler;
using System.Windows;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
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
        private IMainViewModel mainViewModel;

        public App(IMainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Color primaryColor = SwatchHelper.Lookup[MaterialDesignColor.DeepPurple200];
            Color accentColor = SwatchHelper.Lookup[MaterialDesignColor.Blue200];
            ITheme theme = Theme.Create(new MaterialDesignDarkTheme(), primaryColor, accentColor);
            Resources.SetTheme(theme);
            MainWindow mw = new MainWindow(mainViewModel);
            mw.Show();
            base.OnStartup(e);
        }

        public void Launch()
        {
            SplashScreen splashScreen = new SplashScreen("img/smallsplash.png");
            splashScreen.Show(true);
            this.InitializeComponent();
            Run();
        }
    }
}
