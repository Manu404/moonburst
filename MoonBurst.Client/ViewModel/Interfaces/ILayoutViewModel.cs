namespace MoonBurst.ViewModel
{
    public interface ILayoutViewModel
    {
        string CurrentPath { get; set; }
        void Close();
        void LoadLastConfig();
        void DeleteChannel(FunctoidChannelViewModel channel);
    }
}