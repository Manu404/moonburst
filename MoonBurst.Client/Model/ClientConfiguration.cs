using System;
using System.Xml.Serialization;
using MoonBurst.Core;
using MoonBurst.ViewModel;

namespace MoonBurst.Model
{
    public class ClientConfiguration
    {
        public string LastHardwareConfigurationPath { get; set; }
        public string LastLayoutPath { get; set; }
        public string Path { get; set; }

        public void LoadData(ClientConfigurationData data)
        {
            LastHardwareConfigurationPath = data.LastHardwareConfigurationPath;
            LastLayoutPath = data.LastLayoutPath;
            Path = data.Path;
        }

        public ClientConfigurationData GetData()
        {
            return new ClientConfigurationData()
            {
                LastLayoutPath = this.LastLayoutPath,
                LastHardwareConfigurationPath = this.LastHardwareConfigurationPath
            };
        }

        public class ClientConfigurationData : IFileSerializableType
        {
            [XmlIgnore]
            public string Path { get; set; }
            [XmlIgnore]
            public string Default { get => "default_client.xml"; }

            public string LastHardwareConfigurationPath { get; set; }
            public string LastLayoutPath { get; set; }
        }

        public void LoadDefault()
        {
            LoadData(DataSerializer<ClientConfigurationData>.LoadDefault());
        }
    }
}