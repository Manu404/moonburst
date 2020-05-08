using System.Windows;
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
        private static readonly IWindsorContainer _container;

        static PluginEntryPoint()
        {
            if (_container != null) return;
            _container = Bootstrapper.GetDefaultContainer().RegisterDefaultTypesFrom(Classes.FromThisAssembly());
            _container.Register(Component.For<IVstPluginPersistence>().ImplementedBy<PluginPersistence>());
        }
        
        /// <summary>
        /// Called by the framework to create the plugin root class.
        /// </summary>
        /// <returns>Never returns null.</returns>
        protected override IVstPlugin CreatePluginInstance()
        {
            return _container.Resolve<IPluginFactory>().Build(); ;
        }
    }
}
