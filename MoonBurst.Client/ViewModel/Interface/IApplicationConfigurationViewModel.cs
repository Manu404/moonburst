using MoonBurst.Api.Client;
using MoonBurst.Api.Serializer;

namespace MoonBurst.ViewModel.Interface
{
    public interface IApplicationConfigurationViewModel : IViewModel, IFileSerializableType
    {
        string LastHardwareConfigurationPath { get; set; }
        string LastLayoutPath { get; set; }

        void LoadDefault();
        void SaveDefault();
        void Close();
    }
}