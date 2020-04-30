namespace MoonBurst.ViewModel
{
    public interface ILoadSaveDialogProvider
    {
        string ShowLoadDialog(string title, string filter);
        string ShowSaveDialog(string title, string filter);
    }
}