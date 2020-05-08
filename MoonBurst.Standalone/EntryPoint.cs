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
            var app = boot.Resolve<IAppFactory>().Build();
            app.Initialize();
            app.Run();
        }
    }
}
