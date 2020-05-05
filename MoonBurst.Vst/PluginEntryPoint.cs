using Castle.MicroKernel.Registration;
using Castle.Windsor;
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
        private readonly IWindsorContainer _container;

        public PluginEntryPoint()
        {
            _container = new Bootstrapper().GetDefault();
            _container.Register(Component.For<IMainViewHostFactory>().ImplementedBy<VstMainViewHostFactory>());
            _container.Register(Component.For<IApp>().ImplementedBy<App>());
            _container.Register(Component.For<IVstPlugin>().ImplementedBy<Plugin>());
        }

        /// <summary>
        /// Called by the framework to create the plugin root class.
        /// </summary>
        /// <returns>Never returns null.</returns>
        protected override IVstPlugin CreatePluginInstance()
        {
            return _container.Resolve<IVstPlugin>();
        }
    }
}
