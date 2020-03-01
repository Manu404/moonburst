using MoonBurst.Core;
using MoonBurst.ViewModel;

namespace MoonBurst.Model
{
    public class ClientConfigurationViewModelSerializer : SerializerBase<IClientConfigurationViewModel, ClientConfigurationData>
    {
        public override string Default { get => "default_client.xml"; }

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
    }
}