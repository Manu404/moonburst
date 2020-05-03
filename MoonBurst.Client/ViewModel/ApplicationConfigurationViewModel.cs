using GalaSoft.MvvmLight;
using MoonBurst.Core.Serializer;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel
{
    public class ApplicationConfigurationViewModel : ViewModelBase, IApplicationConfigurationViewModel
    {
        private ISerializer<IApplicationConfigurationViewModel> _serializer;

        public string LastHardwareConfigurationPath { get; set; }
        public string LastLayoutPath { get; set; }
        public string CurrentPath { get; set; }

        public ApplicationConfigurationViewModel(ISerializer<IApplicationConfigurationViewModel> serializer)
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