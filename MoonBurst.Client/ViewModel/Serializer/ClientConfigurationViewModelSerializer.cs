using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.Model.Serializable;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Serializer
{
    public class ClientConfigurationViewModelSerializer : SerializerBase<IClientConfigurationViewModel, ClientConfigurationData>
    {
        public override ClientConfigurationData ExtractData(IClientConfigurationViewModel source)
        {
            return new ClientConfigurationData()
            {
                LastLayoutPath = source.LastLayoutPath,
                LastHardwareConfigurationPath = source.LastHardwareConfigurationPath
            };
        }

        public override void ApplyData(ClientConfigurationData config, IClientConfigurationViewModel target)
        {
            target.LastHardwareConfigurationPath = config.LastHardwareConfigurationPath;
            target.LastLayoutPath = config.LastLayoutPath;
        }

        public ClientConfigurationViewModelSerializer() : base("default_client.xml")
        {
        }
    }
}