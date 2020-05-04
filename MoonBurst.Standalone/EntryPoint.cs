using System;
using Castle.MicroKernel.Registration;
using MoonBurst.Api.Client;

namespace MoonBurst
{
    public class EntryPoint
    {
        [STAThread()]
        public static void Main()
        {
            var boot = new Bootstrapper().GetDefault();
            boot.Register(Component.For<IMainViewHostFactory>().ImplementedBy<WindowMainViewHostFactory>());
            boot.Register(Component.For<ILauncher>().ImplementedBy<App>());
            boot.Resolve<ILauncher>().Initialize();
            boot.Resolve<ILauncher>().Run();
        }
    }
}
