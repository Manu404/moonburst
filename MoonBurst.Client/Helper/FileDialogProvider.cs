using System;
using Microsoft.Win32;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{
    public class FileDialogProvider : IFileDialogProvider
    {
        public void ShowSaveDialog(string title, string filter, Action<string> save)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog {Filter = filter, Title = title};
            if (saveFileDialog1.ShowDialog() == true)
            {
                if (saveFileDialog1.FileName != string.Empty)
                {
                    save(saveFileDialog1.FileName);
                }
            }
        }

        public void ShowLoadDialog(string title, string filter, Action<string> load)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog {Filter = filter, Title = title};
            if (openFileDialog1.ShowDialog() == true)
            {
                if (openFileDialog1.FileName != string.Empty)
                {
                    load(openFileDialog1.FileName);
                }
            }
        }
    }
}