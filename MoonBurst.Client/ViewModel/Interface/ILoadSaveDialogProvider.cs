namespace MoonBurst.ViewModel.Interface
{
    public interface IFileDialogProvider
    {
        string ShowLoadDialog(string title, string filter);
        string ShowSaveDialog(string title, string filter);
    }
}