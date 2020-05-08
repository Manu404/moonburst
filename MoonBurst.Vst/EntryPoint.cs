using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

namespace MoonBurst.Vst
{
    /// <summary>
    /// The public Plugin Command Stub implementation derived from the VST.Net framework provided <see cref="StdPluginCommandStub"/>.
    /// </summary>
    public class EntryPoint : StdPluginDeprecatedCommandStub
    {
        private static readonly IWindsorContainer _container;

        static EntryPoint()
        {
            if (_container != null) return;
            _container = Bootstrapper.GetDefaultContainer().RegisterDefaultTypesFrom(Classes.FromThisAssembly());
            _container.Register(Component.For<IVstPluginPersistence>().ImplementedBy<VstPluginPersistence>());
            _container.Register(Component.For<IVstPlugin>().ImplementedBy<VstPlugin>());
        }
        
        /// <summary>
        /// Called by the VST.Net framework to create the plugin root class.
        /// </summary>
        protected override IVstPlugin CreatePluginInstance()
        {
            return _container.Resolve<IVstPlugin>();
        }
    }
}
