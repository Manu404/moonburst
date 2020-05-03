using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Serializer
{
    public class ClientConfigurationViewModelSerializer : SerializerBase<IApplicationConfigurationViewModel, ClientConfigurationModel>
    {
        public override ClientConfigurationModel ExtractData(IApplicationConfigurationViewModel source)
        {
            return new ClientConfigurationModel()
            {
                LastLayoutPath = source.LastLayoutPath,
                LastHardwareConfigurationPath = source.LastHardwareConfigurationPath
            };
        }

        public override void ApplyData(ClientConfigurationModel config, IApplicationConfigurationViewModel target)
        {
            target.LastHardwareConfigurationPath = config.LastHardwareConfigurationPath;
            target.LastLayoutPath = config.LastLayoutPath;
        }

        public ClientConfigurationViewModelSerializer() : base("default_client.xml")
        {
        }
    }
}