using GalaSoft.MvvmLight;
using MoonBurst.Api.Serializer;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{
    public class ApplicationConfigurationViewModel : ViewModelBase, IApplicationConfigurationViewModel
    {
        private readonly ISerializer<IApplicationConfigurationViewModel> _serializer;

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