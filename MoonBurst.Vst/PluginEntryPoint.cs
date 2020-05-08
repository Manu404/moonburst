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
            _container = new Bootstrapper().GetDefault();
            _container.Register(Component.For<IMainViewHostFactory>().ImplementedBy<VstMainViewHostFactory>());
            _container.Register(Component.For<IVstPluginPersistence>().ImplementedBy<PluginPersistence>());
            _container.Register(Component.For<IPluginMidiSourceFactory>().ImplementedBy<PluginMidiSourceFactory>());
            _container.Register(Component.For<IAudioProcessorFactory>().ImplementedBy<AudioProcessorFactory>());
            _container.Register(Component.For<IMidiProcessorFactory>().ImplementedBy<MidiProcessorFactory>());
            _container.Register(Component.For<IPluginEditorFactory>().ImplementedBy<PluginEditorFactory>());
            _container.Register(Component.For<IPluginInterfaceManagerFactory>().ImplementedBy<PluginInterfaceManagerFactory>());
            _container.Register(Component.For<IPluginFactory>().ImplementedBy<PluginFactory>());
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
