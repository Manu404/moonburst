namespace MoonBurst.Model
{
    public interface IClientConfiguration
    {
        string LastHardwareConfigurationPath { get; set; }
        string LastLayoutPath { get; set; }

        void LoadDefault();
        void SaveDefault();
        void Close();
    }
}