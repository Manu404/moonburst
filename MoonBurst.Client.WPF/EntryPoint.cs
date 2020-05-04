using System;
using Castle.MicroKernel.Registration;
using MoonBurst.View;

namespace MoonBurst
{
    public class EntryPoint
    {
        [STAThread()]
        public static void Main()
        {
            var boot = new Bootstrapper().GetDefault();
            boot.Register(Component.For<IMainWindowFactory>().ImplementedBy<MainWindowFactory>());
            boot.Register(Component.For<ILauncher>().ImplementedBy<App>());
            boot.Resolve<ILauncher>().Launch();
        }
    }
}
