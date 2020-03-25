using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Serializer
{
    public class ClientConfigurationViewModelSerializer : SerializerBase<IClientConfigurationViewModel, ClientConfigurationModel>
    {
        public override ClientConfigurationModel ExtractData(IClientConfigurationViewModel source)
        {
            return new ClientConfigurationModel()
            {
                LastLayoutPath = source.LastLayoutPath,
                LastHardwareConfigurationPath = source.LastHardwareConfigurationPath
            };
        }

        public override void ApplyData(ClientConfigurationModel config, IClientConfigurationViewModel target)
        {
            target.LastHardwareConfigurationPath = config.LastHardwareConfigurationPath;
            target.LastLayoutPath = config.LastLayoutPath;
        }

        public ClientConfigurationViewModelSerializer() : base("default_client.xml")
        {
        }
    }
}