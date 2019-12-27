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
        private LayoutViewModel _currentLayout;
        private ClientConfiguration _currentClientConfiguration;
        private HardwareConfigurationViewModel _currentHardwareConfig;
        
        public ICommand OnOpenConsoleCommand { get; set; }
        public ICommand OnCloseCommand { get; set; }

        public HardwareConfigurationViewModel CurrentHardwareConfig
        {
            get => _currentHardwareConfig;
            set
            {
                _currentHardwareConfig = value;
                RaisePropertyChanged();
            }
        }

        public ClientConfiguration CurrentClientConfiguration
        {
            get => _currentClientConfiguration;
            set
            {
                _currentClientConfiguration = value;
                RaisePropertyChanged();
            }
        }

        public LayoutViewModel CurrentLayout
        {
            get => _currentLayout;
            set
            {
                _currentLayout = value;
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
            get => _title.ToUpper() + " - "  + AppVersion + " - " + CurrentLayout?.Path;
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

        public MainViewModel()
        {
            AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Title = "Moon Burst";

            QuickActions = new ObservableCollection<SideMenuAction>();
            
            InitializeConfigs();

            BuildQuickActions();

            OnOpenConsoleCommand = new RelayCommand(OpenConsole);
            OnCloseCommand = new RelayCommand(OnClose);

            WriteLine("using TeVirtualMIDI dll-version:    " + TeVirtualMIDI.versionString);
            WriteLine("using TeVirtualMIDI driver-version: " + TeVirtualMIDI.driverVersionString);

            MessengerInstance.Register<DeleteFunctoidChannelMessage>(this, (d) => this.CurrentLayout.DeleteChannel(d.Item));
        }

        private void BuildQuickActions()
        {
            // TODO SOFTLINK TO LAYOUT
            QuickActions.Clear();
            QuickActions.Add(new SideMenuAction()
            {
                Execute = this.CurrentLayout.OnLoadLayoutCommand,
                Name = "Load Layout",
                TootTip = "Load Layout",
                Kind = PackIconKind.FolderOpen
            });
            QuickActions.Add(new SideMenuAction()
            {
                Execute = this.CurrentLayout.OnSaveLayoutCommand,
                Name = "Save Layout",
                TootTip = "Save Layout",
                Kind = PackIconKind.ContentSave
            });
            QuickActions.Add(new SideMenuAction()
            {
                Execute = this.CurrentLayout.OnSaveAsLayoutCommand,
                Name = "Save Layout As",
                TootTip = "Save Layout As",
                Kind = PackIconKind.ContentSaveAll
            });
            QuickActions.Add(new SideMenuAction()
            {
                Execute = this.CurrentLayout.OnSaveLayoutCommand,
                Name = "Add Functoid",
                TootTip = "Add Functoid",
                Kind = PackIconKind.Plus
            });
            QuickActions.Add(new SideMenuAction()
            {
                Execute = this.CurrentLayout.OnCollaspeAllCommand,
                Name = "Collapse All",
                TootTip = "Collapse All",
                Kind = PackIconKind.ArrowCollapseHorizontal
            });
            QuickActions.Add(new SideMenuAction()
            {
                Execute = this.CurrentLayout.OnExpandAllCommand,
                Name = "Expand All",
                TootTip = "Expand All",
                Kind = PackIconKind.ArrowExpandHorizontal
            });
        }
        private void InitializeConfigs()
        {
            CurrentClientConfiguration = DataSerializer<ClientConfiguration, ClientConfiguration.ClientConfigurationData>.LoadDefault();
            CurrentLayout = DataSerializer<LayoutViewModel, LayoutViewModel.LayoutData>.LoadFromFile(CurrentClientConfiguration.LastLayoutPath);
            CurrentHardwareConfig = DataSerializer<HardwareConfigurationViewModel, HardwareConfigurationViewModel.HardwareConfigurationData>.LoadFromFile(CurrentClientConfiguration.LastHardwareConfigurationPath);
        }

        private void OnClose()
        {
            CurrentLayout.OnSaveLayoutCommand.Execute(null);
            CurrentHardwareConfig.SaveConfigCommand.Execute(null);
            UpdateAndSaveClientConfig();
        }

        private void UpdateAndSaveClientConfig()
        {
            this.CurrentClientConfiguration.LastLayoutPath = this.CurrentLayout.Path;
            this.CurrentClientConfiguration.LastHardwareConfigurationPath = this.CurrentHardwareConfig.Path;
            DataSerializer<ClientConfiguration, ClientConfiguration.ClientConfigurationData>.SaveDefault(this.CurrentClientConfiguration);
            RaisePropertyChanged();
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