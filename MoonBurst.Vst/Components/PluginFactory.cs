using Jacobi.Vst.Framework;
using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
    public interface IPluginFactory : IFactory<IVstPlugin>
    {

    }

    public class PluginFactory : IPluginFactory
    {
        private readonly IMoonburstWpfAppFactory _moonburstWpfAppFactory;
        private readonly IPluginInterfaceManagerFactory _pluginInterfaceFactory;
        private static IVstPlugin _instance;

        public PluginFactory(IMoonburstWpfAppFactory moonburstWpfAppFactory,
            IPluginInterfaceManagerFactory pluginInterfaceFactory)
        {
            this._moonburstWpfAppFactory = moonburstWpfAppFactory;
            _pluginInterfaceFactory = pluginInterfaceFactory;
        }

        public IVstPlugin Build()
        {
            if(_instance == null) _instance = new Plugin(_moonburstWpfAppFactory, _pluginInterfaceFactory);
            return _instance;
        }
    }
}