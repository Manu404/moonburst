using MoonBurst.Api.Client;
using MoonBurst.Api.Serializer;

namespace MoonBurst.ViewModel.Interface
{
    public interface IApplicationConfigurationViewModel : IViewModel, IFileSerializableType
    {
        string LastHardwareConfigurationPath { get; set; }
        string LastLayoutPath { get; set; }
        string LastOrOptionHardwareConfigurationPath { get; }
        string LastOrOptionLayoutPath { get; }

        void LoadDefault();
        void SaveDefault();
        void Close();
    }
}