using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
    using System;
    using System.Drawing;
    using Jacobi.Vst.Core;
    using Jacobi.Vst.Framework;

    class PluginEditor : IVstPluginEditor
    {
        private Plugin _plugin;
        private readonly WpfControlWrapper<VstMainViewHost> _uiWrapper;

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

        public Rectangle Bounds
        {
            get
            {
                _uiWrapper.GetBounds(out var rec);
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
