using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Core.Hardware;
using MoonBurst.Model.Messages;
using MoonBurst.ViewModel.Interfaces;
using MoonBurst.Views;

namespace MoonBurst.ViewModel
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private IClientConfigurationViewModel _clientConfiguration;
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

        public IClientConfigurationViewModel ClientConfiguration
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
            IClientConfigurationViewModel clientConfiguration,
            IHardwareConfigurationViewModel hardwareViewModel,
            ILayoutViewModel layoutViewModel)
        {
            AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Title = "MoonBurst";
            
            _clientConfiguration = clientConfiguration;

            ClientConfiguration = _clientConfiguration;
            HardwareConfig = hardwareViewModel;
            Layout = layoutViewModel;
            
            InitializeConfigs();

            OnOpenConsoleCommand = new RelayCommand(OpenConsole);
            OnCloseCommand = new RelayCommand(OnClose);

            WriteLine("using TeVirtualMIDI dll-version:    " + TeVirtualMidi.VersionString);
            WriteLine("using TeVirtualMIDI driver-version: " + TeVirtualMidi.DriverVersionString);

            messenger.Register<DeleteFunctoidChannelMessage>(this, (d) => this.Layout.DeleteChannel(d.Item));
        }

        private void InitializeConfigs()
        {
            ClientConfiguration.LoadDefault();
            HardwareConfig.LoadLast();
            Layout.LoadLast();
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
}