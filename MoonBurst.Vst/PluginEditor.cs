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
            return new TestUi();
        }
    }

    class PluginEditor : IVstPluginEditor
    {
        private Plugin _plugin;

        private WpfControlWrapper<TestUi> _uiWrapper;
        //private WinFormsControlWrapper<WpfHostForm> _uiWrapper = new WinFormsControlWrapper<WpfHostForm>();

        public PluginEditor(Plugin plugin)
        {
            _plugin = plugin;
            var boot = new Bootstrapper().GetDefault();
            boot.Register(Component.For<IMainWindowFactory>().ImplementedBy<MainViewHostFactory>());
            boot.Register(Component.For<ILauncher>().ImplementedBy<App>());
            var l = boot.Resolve<ILauncher>();
            l.Initialize();
            _uiWrapper = new WpfControlWrapper<TestUi>(800, 600, (TestUi)l.Host);
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
            //_uiWrapper.Instance.NoteMap = _plugin.NoteMap;
            //_uiWrapper.SafeInstance.NoteOnEvents = _plugin.GetInstance<MidiProcessor>().NoteOnEvents;
            _uiWrapper.Open(hWnd);
        }

        public void ProcessIdle()
        {
           // _uiWrapper.SafeInstance.Proc();
        }


        #endregion
    }
}
