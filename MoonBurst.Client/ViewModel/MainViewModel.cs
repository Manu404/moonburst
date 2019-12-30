using GalaSoft.MvvmLight;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MoonBurst.Core;
using MoonBurst.Model;
using MoonBurst.Views;
using ChannelCommand = MoonBurst.Model.ChannelCommand;

namespace MoonBurst.ViewModel
{
    public class SideMenuAction
    {
        public PackIconKind Kind { get; set; }
        public ICommand Execute { get; set; }
        public string TootTip { get; set; }
        public string Name { get; set; }
    }

    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<SideMenuAction> QuickActions { get; set; }

        private DebugWindow debugWindow;
        private string _log;
        private string _appVersion;
        private string _title;
        private LayoutViewModel _layout;
        private ClientConfiguration _clientConfiguration;
        private HardwareConfigurationViewModel _hardwareConfig;
        
        public ICommand OnOpenConsoleCommand { get; set; }
        public ICommand OnCloseCommand { get; set; }
        
        public HardwareConfigurationViewModel HardwareConfig
        {
            get => _hardwareConfig;
            set
            {
                _hardwareConfig = value;
                RaisePropertyChanged();
            }
        }

        public ClientConfiguration ClientConfiguration
        {
            get => _clientConfiguration;
            set
            {
                _clientConfiguration = value;
                RaisePropertyChanged();
            }
        }

        public LayoutViewModel Layout
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
            get => _title.ToUpper() + " - "  + AppVersion + " - " + Layout?.Path;
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

        private IArduinoGateway _arduinoGateway;
        private IMessenger _messenger;

        public MainViewModel()
        {
            AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Title = "Moon Burst";
            
            _arduinoGateway = new ArduinoGateway();
            _messenger = MessengerInstance;

            ClientConfiguration = new ClientConfiguration();
            HardwareConfig = new HardwareConfigurationViewModel(new MidiGateway(_messenger), new SerialGateway(_messenger), _arduinoGateway, _messenger);
            Layout = new LayoutViewModel(_arduinoGateway, _messenger);
            QuickActions = new ObservableCollection<SideMenuAction>();
            
            InitializeConfigs();

            BuildQuickActions();

            OnOpenConsoleCommand = new RelayCommand(OpenConsole);
            OnCloseCommand = new RelayCommand(OnClose);

            WriteLine("using TeVirtualMIDI dll-version:    " + TeVirtualMIDI.versionString);
            WriteLine("using TeVirtualMIDI driver-version: " + TeVirtualMIDI.driverVersionString);

            _messenger.Register<DeleteFunctoidChannelMessage>(this, (d) => this.Layout.DeleteChannel(d.Item));
        }

        private void BuildQuickActions()
        {
        }

        private void InitializeConfigs()
        {
            ClientConfiguration.LoadDefault();
            HardwareConfig.LoadData(DataSerializer<HardwareConfigurationViewModel.HardwareConfigurationData>.LoadFromFile(ClientConfiguration.LastHardwareConfigurationPath));
            Layout.LoadData(DataSerializer<LayoutViewModel.LayoutData>.LoadFromFile(ClientConfiguration.LastLayoutPath));
        }

        private void UpdateAndSaveClientConfig()
        {
            this.ClientConfiguration.LastLayoutPath = this.Layout.Path;
            this.ClientConfiguration.LastHardwareConfigurationPath = this.HardwareConfig.Path;
            DataSerializer<ClientConfiguration.ClientConfigurationData>.SaveDefault(this.ClientConfiguration.GetData());
            RaisePropertyChanged();
        }

        private void OnClose()
        {
            Layout.OnSaveLayoutCommand.Execute(null);
            HardwareConfig.SaveConfigCommand.Execute(null);
            UpdateAndSaveClientConfig();
        }

        void OpenConsole()
        {
            if (debugWindow == null)
            {
                debugWindow = new DebugWindow(this);
                debugWindow.Closed += (sender, args) => this.debugWindow = null;
            }

            debugWindow.Show();
        }

        #region utils
        void WriteLine(string newLine)
        {
            this.Log += newLine + "\n";
        }
        #endregion
    }
}