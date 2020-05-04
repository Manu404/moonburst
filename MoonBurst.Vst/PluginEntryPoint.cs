using Castle.MicroKernel.Registration;
using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
    using Jacobi.Vst.Framework;
    using Jacobi.Vst.Framework.Plugin;

    /// <summary>
    /// The public Plugin Command Stub implementation derived from the framework provided <see cref="StdPluginCommandStub"/>.
    /// </summary>
    public class PluginEntryPoint : StdPluginDeprecatedCommandStub
    {
        /// <summary>
        /// Called by the framework to create the plugin root class.
        /// </summary>
        /// <returns>Never returns null.</returns>
        protected override IVstPlugin CreatePluginInstance()
        {
            var boot = new Bootstrapper().GetDefault();
            boot.Register(Component.For<IMainViewHostFactory>().ImplementedBy<MainViewHostFactory>());
            boot.Register(Component.For<ILauncher>().ImplementedBy<App>());
            boot.Register(Component.For<IVstPlugin>().ImplementedBy<Plugin>());
            return boot.Resolve<IVstPlugin>();
        }
    }
}
