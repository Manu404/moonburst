using System.ComponentModel;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using MoonBurst.View;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private IApplicationConfigurationViewModel _applicationConfiguration;
        private IHardwareConfigurationViewModel _hardwareConfig;
        private ILayoutViewModel _layout;

        private DebugWindow _debugWindow;
        private string _log;
        private string _appVersion;
        private string _title;
        private string _statusBarText;

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

        public IApplicationConfigurationViewModel ApplicationConfiguration
        {
            get => _applicationConfiguration;
            set
            {
                _applicationConfiguration = value;
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

        public string StatusBarText
        {
            get => _statusBarText;
            set
            {
                _statusBarText = value;
                RaisePropertyChanged();
            }
        }


        public MainViewModel(IApplicationConfigurationViewModel applicationConfiguration,
            IHardwareConfigurationViewModel hardwareViewModel,
            ILayoutViewModel layoutViewModel)
        {
            AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Title = "MoonBurst";
            
            _applicationConfiguration = applicationConfiguration;

            ApplicationConfiguration = _applicationConfiguration;
            HardwareConfig = hardwareViewModel;
            Layout = layoutViewModel;
            if (HardwareConfig is ViewModelBase) ((ViewModelBase)HardwareConfig).PropertyChanged += OnHardwarePropertyChange;

            InitializeConfigs();

            OnOpenConsoleCommand = new RelayCommand(OpenConsole);
            OnCloseCommand = new RelayCommand(OnClose);

            //WriteLine("using TeVirtualMIDI dll-version:    " + TeVirtualMidi.VersionString);
            //WriteLine("using TeVirtualMIDI driver-version: " + TeVirtualMidi.DriverVersionString);
        }

        private void OnHardwarePropertyChange(object sender, PropertyChangedEventArgs e)
        {
            this.StatusBarText = HardwareConfig.GetStatusString();
        }

        private void InitializeConfigs()
        {
            ApplicationConfiguration.LoadDefault();
            HardwareConfig.LoadLast();
            Layout.LoadLast();
        }

        private void OnClose()
        {
            this.Layout.Close();
            this.HardwareConfig.Close();
            this.ApplicationConfiguration.Close();
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