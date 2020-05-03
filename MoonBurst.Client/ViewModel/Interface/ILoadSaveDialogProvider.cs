﻿namespace MoonBurst.ViewModel.Interface
{
    public interface ILoadSaveDialogProvider
    {
        string ShowLoadDialog(string title, string filter);
        string ShowSaveDialog(string title, string filter);
    }
}