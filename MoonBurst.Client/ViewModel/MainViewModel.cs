using System;
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
        private DebugWindow _debugWindow;
        private string _title;

        public ICommand OnOpenConsoleCommand { get; set; }
        public ICommand OnCloseCommand { get; set; }
        
        public IHardwareConfigurationViewModel HardwareConfig { get; set; }
        public IApplicationConfigurationViewModel ApplicationConfiguration { get; set; }
        public ILayoutViewModel Layout { get; set; }

        public string AppVersion { get; set; }
        public string Log { get; set; }
        public string StatusBarText { get; set; }

        public string Title
        {
            get =>$"{_title.ToUpper()} - {AppVersion} - {Layout?.CurrentPath} - {HardwareConfig?.CurrentPath} ";
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }
        
        public MainViewModel(
            IApplicationConfigurationViewModel applicationConfiguration,
            IHardwareConfigurationViewModel hardwareViewModel,
            ILayoutViewModel layoutViewModel)
        {
            AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Title = "MoonBurst";
            
            ApplicationConfiguration = applicationConfiguration;
            HardwareConfig = hardwareViewModel;
            Layout = layoutViewModel;
            HardwareConfig.ConfigurationChanged += HardwareConfigOnConfigurationChanged;

            InitializeConfigs();

            OnOpenConsoleCommand = new RelayCommand(OpenConsole);
            OnCloseCommand = new RelayCommand(OnClose);
        }

        private void HardwareConfigOnConfigurationChanged(object sender, EventArgs e)
        {
            StatusBarText = HardwareConfig.GetStatusString();
        }

        private void InitializeConfigs()
        {
            ApplicationConfiguration.LoadDefault();
            HardwareConfig.LoadLast();
            Layout.LoadLast();
        }

        private void OnClose()
        {
            Layout.Close();
            HardwareConfig.Close();
            ApplicationConfiguration.Close();
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