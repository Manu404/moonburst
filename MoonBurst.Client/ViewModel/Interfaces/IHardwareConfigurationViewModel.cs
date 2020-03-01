namespace MoonBurst.ViewModel
{
    public interface IHardwareConfigurationViewModel
    {
        string CurrentPath { get; set; }
        void Close();
        void LoadLastConfig();
    }
}