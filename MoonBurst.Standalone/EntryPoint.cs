using System;
using Castle.MicroKernel.Registration;
using MoonBurst.Api.Client;
using MoonBurst.Core;

namespace MoonBurst
{
    public class EntryPoint
    {
        [STAThread()]
        public static void Main(string[] args)
        {
            var instanceHelper = new InstanceHelper();
            if (!instanceHelper.IsSingleInstance())
               if(instanceHelper.SetExstingProcessForegroundWindow())
                    Environment.Exit(0);            

            var boot = Bootstrapper.GetDefaultContainer();
            boot.Register(Component.For<IMainViewHostFactory>().ImplementedBy<WindowMainViewHostFactory>());
            boot.Register(Component.For<IStartupOptionParser>().Instance(new CommandLineStartOptionParser(args)));
            var app = boot.Resolve<IMoonburstWpfAppFactory>().Build();
            app.Initialize();
            app.Run();
        }
    }
}
