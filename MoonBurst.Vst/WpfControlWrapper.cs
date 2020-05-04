﻿namespace MoonBurst.Vst
{
    using System;
    using System.Drawing;
    using System.Windows.Controls;
    using System.Windows.Interop;

    class WpfControlWrapper<T> where T : UserControl, new()
    {
        private HwndSource _hwndSource;
        private int _width;
        private int _height;
        
        public WpfControlWrapper(int width, int height, T instance)
        {
            _width = width;
            _height = height;
            _instance = instance;
        }

        private T _instance;

        /// <summary>
        /// Opens and attaches the Control to the <paramref name="hWnd"/>.
        /// </summary>
        /// <param name="hWnd">The native win32 handle to the main window of the host.</param>
        public void Open(IntPtr hWnd)
        {
            if (_instance == null) return;

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
}
