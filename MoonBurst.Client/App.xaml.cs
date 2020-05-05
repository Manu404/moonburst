using System.Reflection;
using System.Windows;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using MoonBurst.Api.Client;

namespace MoonBurst
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : IApp
    {
        public IMainViewHost Host { get; private set; }

        private readonly IMainViewHostFactory _mainViewHostFactory;

        public App(IMainViewHostFactory mainViewHostFactory)
        {
            this._mainViewHostFactory = mainViewHostFactory;
        }

        public void Initialize()
        {
            SplashScreen splashScreen = new SplashScreen(Assembly.GetAssembly(GetType()),"img/smallsplash.png");
            splashScreen.Show(true);
            this.InitializeComponent();
            Color primaryColor = SwatchHelper.Lookup[MaterialDesignColor.DeepPurple200];
            Color accentColor = SwatchHelper.Lookup[MaterialDesignColor.Blue200];
            ITheme theme = Theme.Create(new MaterialDesignDarkTheme(), primaryColor, accentColor);
            Resources.SetTheme(theme);
            Host = _mainViewHostFactory.Build();
        }

        public new void Run()
        {
            Host.Start();
            base.Run();
        }

    }
}
