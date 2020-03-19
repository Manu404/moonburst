namespace MoonBurst.Vst
{
    using System;
    using System.Drawing;
    using System.Windows.Controls;
    using System.Windows.Interop;
    using Jacobi.Vst.Core;
    using Jacobi.Vst.Framework;
    using Jacobi.Vst.Framework.Common;


    /// <summary>
    /// Helper class to attach a WPF UserControl to a Win32 native main window of the host.
    /// </summary>
    /// <typeparam name="T">The type of the managed WPF Control.</typeparam>
    class WpfControlWrapper<T> where T : UserControl, new()
    {
        private HwndSource _hwndSource;
        private int _width;
        private int _height;

        /// <summary>
        /// Constructs a new instance for the specified <paramref name="width"/> and <paramref name="height"/>.
        /// </summary>
        /// <param name="width">The width of the control.</param>
        /// <param name="height">The height of the control.</param>
        public WpfControlWrapper(int width, int height)
        {
            _width = width;
            _height = height;
        }

        private T _instance;
        /// <summary>
        /// Gets and instance of the specified <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>Can return null.</remarks>
        public T Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Opens and attaches the Control to the <paramref name="hWnd"/>.
        /// </summary>
        /// <param name="hWnd">The native win32 handle to the main window of the host.</param>
        public void Open(IntPtr hWnd)
        {
            _instance = new T();
            _instance.Width = _width;
            _instance.Height = _height;

            HwndSourceParameters hwndParams = new HwndSourceParameters("VST.NET Wpf Editor");
            hwndParams.ParentWindow = hWnd;
            hwndParams.Height = _height;
            hwndParams.Width = _width;
            hwndParams.WindowStyle = 0x10000000 | 0x40000000; // WS_VISIBLE|WS_CHILD

            _hwndSource = new HwndSource(hwndParams);
            _hwndSource.RootVisual = _instance;
        }

        /// <summary>
        /// Returns the bounding rectangle of the Control.
        /// </summary>
        /// <param name="rect">Receives the bounding rectangle.</param>
        /// <remarks>The same size as in design-time.</remarks>
        public void GetBounds(out Rectangle rect)
        {
            rect = new Rectangle(0, 0, _width, _height);
        }

        /// <summary>
        /// Closes and destroys the Control.
        /// </summary>
        public void Close()
        {
            if (_hwndSource != null)
            {
                _hwndSource.Dispose();
                _hwndSource = null;
            }

            _instance = null;
        }
    }

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
