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
            boot.Register(Component.For<IApp>().ImplementedBy<App>());
            boot.Resolve<IApp>().Initialize();
            boot.Resolve<IApp>().Run();
        }
    }
}
