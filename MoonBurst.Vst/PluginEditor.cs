namespace MoonBurst.Vst
{
    using System;
    using System.Drawing;
    using Jacobi.Vst.Core;
    using Jacobi.Vst.Framework;
    using Jacobi.Vst.Framework.Common;

    /// <summary>
    /// Implements the custom UI editor for the plugin.
    /// </summary>
    class PluginEditor : IVstPluginEditor
    {
        private Plugin _plugin;
        private WpfControlWrapper<TestUi> _uiWrapper = new WpfControlWrapper<TestUi>(800, 600);
        //private WinFormsControlWrapper<WpfHostForm> _uiWrapper = new WinFormsControlWrapper<WpfHostForm>();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="plugin">Must not be null.</param>
        public PluginEditor(Plugin plugin)
        {
            _plugin = plugin;
        }

        #region IVstPluginEditor Members

        public void Close()
        {
            _uiWrapper.Close();
        }

        public bool KeyDown(byte ascii, VstVirtualKey virtualKey, VstModifierKeys modifers)
        {
            // no-op
            return false;
        }

        public bool KeyUp(byte ascii, VstVirtualKey virtualKey, VstModifierKeys modifers)
        {
            // no-op
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
