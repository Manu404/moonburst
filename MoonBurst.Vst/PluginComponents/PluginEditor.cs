using System.Windows.Forms;
using MoonBurst.Api.Client;
using System;
using System.Drawing;
using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;

namespace MoonBurst.Vst
{
    public interface IPluginEditorFactory : IFactory<IVstPluginEditor, IMainViewHost>
    {
    }

    public class PluginEditorFactory : IPluginEditorFactory
    {
        public IVstPluginEditor Build(IMainViewHost host)
        {
            return new PluginEditor(host);
        }
    }

    class PluginEditor : IVstPluginEditor
    {
        private MainViewHostControlWrapper _uiWrapper;

        public PluginEditor(IMainViewHost host)
        {
            _uiWrapper = new MainViewHostControlWrapper(800, 600, host);
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
            try
            {
                _uiWrapper.Open(hWnd);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ProcessIdle()
        {
        }

        #endregion
    }
}
