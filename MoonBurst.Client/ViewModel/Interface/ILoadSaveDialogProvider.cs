using System;

namespace MoonBurst.ViewModel.Interface
{
    public interface IFileDialogProvider
    {
        void ShowLoadDialog(string title, string filter, Action<string> save);
        void ShowSaveDialog(string title, string filter, Action<string> load);
    }
}