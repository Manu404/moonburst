using MoonBurst.Core;
using MoonBurst.Model;

namespace MoonBurst.ViewModel
{
    public interface IClientConfigurationViewModel : IViewModel, IFileSerializableType
    {
        string LastHardwareConfigurationPath { get; set; }
        string LastLayoutPath { get; set; }

        void LoadDefault();
        void SaveDefault();
        void Close();
    }
}