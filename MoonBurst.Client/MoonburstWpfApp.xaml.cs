using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using MoonBurst.Api.Client;

namespace MoonBurst
{
    public interface IMoonburstWpfAppFactory : IFactory<IApp>
    {

    }
    public class MoonburstWpfAppFactory : IMoonburstWpfAppFactory
    {
        private static IApp _instance;
        private readonly IMainViewHostFactory _mainViewHostFactory;

        public MoonburstWpfAppFactory(IMainViewHostFactory mainViewHostFactory)
        {
            _mainViewHostFactory = mainViewHostFactory;
        }
        public IApp Build()
        {
            if (Application.Current == null && _instance == null) _instance = new MoonburstWpfApp(_mainViewHostFactory);
            return _instance;
        }
    }

    public partial class MoonburstWpfApp : IApp
    {
        private bool isInitialized = false;
        public IMainViewHost Host { get; private set; }

        private readonly IMainViewHostFactory _mainViewHostFactory;

        public MoonburstWpfApp(IMainViewHostFactory mainViewHostFactory)
        {
            this._mainViewHostFactory = mainViewHostFactory;
        }

        public void Initialize()
        {
            if (isInitialized) return;
            SplashScreen splashScreen = new SplashScreen(Assembly.GetAssembly(GetType()),"img/smallsplash.png");
            splashScreen.Show(true);
            this.InitializeComponent();
            Color primaryColor = SwatchHelper.Lookup[MaterialDesignColor.DeepPurple200];
            Color accentColor = SwatchHelper.Lookup[MaterialDesignColor.Blue200];
            ITheme theme = Theme.Create(new MaterialDesignDarkTheme(), primaryColor, accentColor);
            Resources.SetTheme(theme);
            Host = _mainViewHostFactory.Build();
            isInitialized = true;
        }

        public new void Run()
        {
            base.Run(Host as Window);
        }
    }
}
