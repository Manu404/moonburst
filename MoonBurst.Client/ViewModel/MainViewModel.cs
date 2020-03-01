using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Core;
using MoonBurst.Model;
using MoonBurst.Model.Messages;
using MoonBurst.Views;

namespace MoonBurst.ViewModel
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private IMessenger _messenger;

        private IClientConfiguration _clientConfiguration;
        private IHardwareConfigurationViewModel _hardwareConfig;
        private ILayoutViewModel _layout;

        private DebugWindow _debugWindow;
        private string _log;
        private string _appVersion;
        private string _title;
        
        public ICommand OnOpenConsoleCommand { get; set; }
        public ICommand OnCloseCommand { get; set; }
        
        public IHardwareConfigurationViewModel HardwareConfig
        {
            get => _hardwareConfig;
            set
            {
                _hardwareConfig = value;
                RaisePropertyChanged();
            }
        }

        public IClientConfiguration ClientConfiguration
        {
            get => _clientConfiguration;
            set
            {
                _clientConfiguration = value;
                RaisePropertyChanged();
            }
        }

        public ILayoutViewModel Layout
        {
            get => _layout;
            set
            {
                _layout = value;
                RaisePropertyChanged();
            }
        }

        public string AppVersion
        {
            get => _appVersion;
            set
            {
                _appVersion = value;
                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get =>$"{_title.ToUpper()} - {AppVersion} - {Layout?.CurrentPath} - {HardwareConfig?.CurrentPath} ";
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        public string Log
        {
            get => _log;
            set
            {
                _log = value;
                RaisePropertyChanged();
            }
        }


        public MainViewModel(IMessenger messenger,  
            IClientConfiguration clientConfiguration,
            IHardwareConfigurationViewModel hardware_viewModel,
            ILayoutViewModel layout_viewModel)
        {
            AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Title = "MoonBurst";
            
            _messenger = messenger;
            _clientConfiguration = clientConfiguration;

            ClientConfiguration = _clientConfiguration;
            HardwareConfig = hardware_viewModel;
            Layout = layout_viewModel;
            
            InitializeConfigs();

            OnOpenConsoleCommand = new RelayCommand(OpenConsole);
            OnCloseCommand = new RelayCommand(OnClose);

            WriteLine("using TeVirtualMIDI dll-version:    " + TeVirtualMIDI.versionString);
            WriteLine("using TeVirtualMIDI driver-version: " + TeVirtualMIDI.driverVersionString);

            _messenger.Register<DeleteFunctoidChannelMessage>(this, (d) => this.Layout.DeleteChannel(d.Item));
        }

        private void InitializeConfigs()
        {
            ClientConfiguration.LoadDefault();
            HardwareConfig.LoadLastConfig();
            Layout.LoadLastConfig();
        }

        private void OnClose()
        {
            this.Layout.Close();
            this.HardwareConfig.Close();
            this.ClientConfiguration.Close();
        }

        void OpenConsole()
        {
            if (_debugWindow == null)
            {
                _debugWindow = new DebugWindow(this);
                _debugWindow.Closed += (sender, args) => this._debugWindow = null;
            }

            _debugWindow.Show();
        }

        #region utils
        void WriteLine(string newLine)
        {
            this.Log += newLine + "\n";
        }
        #endregion
    }

    public interface IMainViewModel
    {
    }
}