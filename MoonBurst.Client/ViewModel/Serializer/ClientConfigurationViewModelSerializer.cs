using MoonBurst.Core;

namespace MoonBurst.Model
{
    public partial class ClientConfigurationViewModelSerializer : SerializerBase<ClientConfigurationViewModel, ClientConfigurationData>
    {
        public override string Default { get => "default_client.xml"; }

        public override ClientConfigurationData ExtractData(ClientConfigurationViewModel source)
        {
            return new ClientConfigurationData()
            {
                LastLayoutPath = source.LastLayoutPath,
                LastHardwareConfigurationPath = source.LastHardwareConfigurationPath
            };
        }

        public override void ApplyData(ClientConfigurationData config, ClientConfigurationViewModel target)
        {
            target.LastHardwareConfigurationPath = config.LastHardwareConfigurationPath;
            target.LastLayoutPath = config.LastLayoutPath;
        }
    }
}