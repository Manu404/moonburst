using System;
using System.Windows.Forms;
using MoonBurst.Api.Client;
using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;
using System.Reflection;

namespace MoonBurst.Vst
{
    class Plugin : VstPluginBase
    {
        private readonly IApp _app;
        private readonly PluginInterfaceManager _interfaceManager;

        public Plugin(IMoonburstWpfAppFactory moonburstWpfAppFactory,
            IPluginInterfaceManagerFactory interfaceManagerFactory)
            : base("MoonBurst", 
                new VstProductInfo("MoonBurst " + Assembly.GetExecutingAssembly().GetName().Version, "Emmanuel Istace", 1001),
                VstPluginCategory.Generator, 
                VstPluginCapabilities.NoSoundInStop, 
                0, 
                0x30313234)
        {
            this._app = moonburstWpfAppFactory.Build();
            this._app.Initialize();
            _interfaceManager = interfaceManagerFactory.Build(new Tuple<IApp, IVstHost>(_app, this.Host));
        }

        public override bool Supports<T>()
        {
            return _interfaceManager.Supports<T>();
        }
        public override T GetInstance<T>()
        {
            return _interfaceManager.GetInstance<T>();
        }
    }
}
