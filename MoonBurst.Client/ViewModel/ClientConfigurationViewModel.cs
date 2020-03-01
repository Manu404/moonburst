using System;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using MoonBurst.Core;
using MoonBurst.ViewModel;

namespace MoonBurst.Model
{
    public partial class ClientConfigurationViewModel : ViewModelBase, IClientConfiguration, IFileSerializableType
    {
        public string LastHardwareConfigurationPath { get; set; }
        public string LastLayoutPath { get; set; }
        public string CurrentPath { get; set; }

        private ISerializer<ClientConfigurationViewModel> _serializer;

        public ClientConfigurationViewModel(ISerializer<ClientConfigurationViewModel> serializer)
        {
            _serializer = serializer;
        }

        public void LoadDefault()
        {
            _serializer.LoadDefault(this);
        }

        public void SaveDefault()
        {
            _serializer.SaveDefault(this);
        }

        public void Close()
        {
            SaveDefault();
        }
    }
}