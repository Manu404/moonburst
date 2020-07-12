using GalaSoft.MvvmLight;
using MoonBurst.Api.Client;
using MoonBurst.Api.Serializer;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{
    public class ApplicationConfigurationViewModel : ViewModelBase, IApplicationConfigurationViewModel
    {
        private readonly ISerializer<IApplicationConfigurationViewModel> _serializer;
        private readonly IStartupOptionParser _options;

        public string LastHardwareConfigurationPath { get; set; }
        public string LastLayoutPath { get; set; }
        public string CurrentPath { get; set; }

        public string LastOrOptionHardwareConfigurationPath 
        { 
            get 
            {
                return _options.Get().Config ?? LastHardwareConfigurationPath;
            } 
        }
        public string LastOrOptionLayoutPath
        {
            get
            {
                return _options.Get().Layout ?? LastLayoutPath;
            }
        }

        public ApplicationConfigurationViewModel(ISerializer<IApplicationConfigurationViewModel> serializer, IStartupOptionParser options)
        {
            _serializer = serializer;
            _options = options;
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