using System.Reflection;
using System.Windows;
using Castle.MicroKernel.Registration;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using MoonBurst.Core;
using MoonBurst.View;
using MoonBurst.ViewModel.Interface;
using MoonBurst.Vst.Properties;

namespace MoonBurst.Vst
{
    using System;
    using System.Drawing;
    using Jacobi.Vst.Core;
    using Jacobi.Vst.Framework;
    using Jacobi.Vst.Framework.Common;

    public class MainViewHostFactory : IMainWindowFactory
    {
        private IMainViewModel mainViewModel;
        private IFactory<IMainView> viewFactory;

        public MainViewHostFactory(IMainViewModel mainViewModel, IFactory<IMainView> viewFactory)
        {
            this.mainViewModel = mainViewModel;
            this.viewFactory = viewFactory;
        }
        public IMainViewHost Build()
        {
            return new VstMainViewHost(mainViewModel, viewFactory);
        }
    }

    class PluginEditor : IVstPluginEditor
    {
        private Plugin _plugin;
        private WpfControlWrapper<VstMainViewHost> _uiWrapper;

        public PluginEditor(Plugin plugin, IMainViewHost host)
        {
            _plugin = plugin;

            _uiWrapper = new WpfControlWrapper<VstMainViewHost>(800, 600, (VstMainViewHost)host);
        }

        #region IVstPluginEditor Members

        public void Close()
        {
            _uiWrapper.Close();
        }

        public bool KeyDown(byte ascii, VstVirtualKey virtualKey, VstModifierKeys modifers)
        {
            return false;
        }

        public bool KeyUp(byte ascii, VstVirtualKey virtualKey, VstModifierKeys modifers)
        {
            return false;
        }

        public VstKnobMode KnobMode { get; set; }

        public System.Drawing.Rectangle Bounds
        {
            get
            {
                Rectangle rec;
                _uiWrapper.GetBounds(out rec);
                return rec;
            }
        }

        public void Open(IntPtr hWnd)
        {
            _uiWrapper.Open(hWnd);
        }

        public void ProcessIdle()
        {
        }

        #endregion
    }
}
