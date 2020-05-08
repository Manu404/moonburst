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

    public class VstPluginEditorFactory : IPluginEditorFactory
    {
        private IVstPluginEditor _instance;
        public IVstPluginEditor Build(IMainViewHost mainViewHost)
        {
            if(_instance == null) _instance = new VstPluginEditor(mainViewHost);
            return _instance;
        }
    }

    class VstPluginEditor : IVstPluginEditor
    {
        private readonly MainViewHostControlWrapper _uiWrapper;

        public VstPluginEditor(IMainViewHost mainViewHost)
        {
            _uiWrapper = new MainViewHostControlWrapper(800, 600, mainViewHost);
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
