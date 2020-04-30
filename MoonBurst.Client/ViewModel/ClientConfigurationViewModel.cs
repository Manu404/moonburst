using GalaSoft.MvvmLight;
using MoonBurst.Core.Serializer;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel
{
    public class ClientConfigurationViewModel : ViewModelBase, IClientConfigurationViewModel
    {
        private ISerializer<IClientConfigurationViewModel> _serializer;

        public string LastHardwareConfigurationPath { get; set; }
        public string LastLayoutPath { get; set; }
        public string CurrentPath { get; set; }

        public ClientConfigurationViewModel(ISerializer<IClientConfigurationViewModel> serializer)
        {
            _serializer = serializer;
        }

        public void Close()
        {
            SaveDefault();
        }

        public void LoadDefault()
        {
            _serializer.LoadDefault(this);
        }

        public void SaveDefault()
        {
            _serializer.SaveDefault(this);
        }
    }
}