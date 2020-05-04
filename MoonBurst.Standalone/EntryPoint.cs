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
            boot.Register(Component.For<IMainWindowFactory>().ImplementedBy<MainViewHostFactory>());
            boot.Register(Component.For<ILauncher>().ImplementedBy<App>());
            boot.Resolve<ILauncher>().Initialize();
            boot.Resolve<ILauncher>().Run();
        }
    }
}
