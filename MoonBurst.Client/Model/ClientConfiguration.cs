using System;

namespace MoonBurst.ViewModel
{


    public class ClientConfiguration : IFileSerializableType<ClientConfiguration.ClientConfigurationData>
    {
        public string LastHardwareConfigurationPath { get; set; }
        public string LastLayoutPath { get; set; }

        public ClientConfiguration()
        {

        }

        public ClientConfiguration(ClientConfigurationData data, string path) : this()
        {
            LastHardwareConfigurationPath = data.LastHardwareConfigurationPath;
            LastLayoutPath = data.LastLayoutPath;
            Path = path;
        }

        public string Path { get; set; }
        public string Default { get => "default_client.xml"; }

        public ClientConfigurationData GetData()
        {
            return new ClientConfigurationData()
            {
                LastLayoutPath = this.LastLayoutPath,
                LastHardwareConfigurationPath = this.LastHardwareConfigurationPath
            };
        }

        public IFileSerializableType<ClientConfigurationData> CreateFromData(ClientConfigurationData data, string path)
        {
            return new ClientConfiguration(data, path);
        }

        public class ClientConfigurationData : IFileSerializableData
        {

            public string LastHardwareConfigurationPath { get; set; }
            public string LastLayoutPath { get; set; }
        }
    }
}