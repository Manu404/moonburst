using Jacobi.Vst.Framework;
using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
    public interface IPluginFactory : IFactory<IVstPlugin>
    {

    }

    public class PluginFactory : IPluginFactory
    {
        private readonly IAppFactory _appFactory;
        private readonly IPluginInterfaceManagerFactory _pluginInterfaceFactory;
        private static IVstPlugin _instance;

        public PluginFactory(IAppFactory appFactory,
            IPluginInterfaceManagerFactory pluginInterfaceFactory)
        {
            this._appFactory = appFactory;
            _pluginInterfaceFactory = pluginInterfaceFactory;
        }

        public IVstPlugin Build()
        {
            if(_instance == null) _instance = new Plugin(_appFactory, _pluginInterfaceFactory);
            return _instance;
        }
    }
}