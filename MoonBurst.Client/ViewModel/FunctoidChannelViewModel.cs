using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Services;
using MoonBurst.Core.Helper;
using MoonBurst.Helper.UI;
using MoonBurst.Model.Messages;
using MoonBurst.ViewModel.Factories;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel
{
    public class FunctoidChannelViewModel : ViewModelBase, IFunctoidChannelViewModel
    {
        private IArduinoGateway _arduinoGateway;
        private IMessenger _messenger;
        private IFunctoidActionViewModelFactory _actionFactory;
        private string _lastInput;
        
        private int _index;
        private string _name;
        private bool _isEnabled;
        private bool _isExpanded;
        private bool _isTriggered;
        private DeviceInputViewModel _selectedInput;
        
        public DeviceInputViewModel SelectedInput
        {
            get => _selectedInput;
            set
            {
                _selectedInput = value;
                if (value != null)
                    _lastInput = value.FormatedName;
                RaisePropertyChanged();
            }
        }

        public bool IsTriggered
        {
            get => _isTriggered;
            set
            {
                _isTriggered = value;
                RaisePropertyChanged();
                _isTriggered = !value;
                RaisePropertyChanged();
            }
        }

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                RaisePropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IFunctoidActionViewModel> Actions { get; set; }
        public ObservableCollection<DeviceInputViewModel> AvailableInputs { get; set; }

        public int ArduinoBitMask { get; set; }

        public ICommand OnDeleteCommand { get; set; }
        public ICommand OnAddActionCommand { get; set; }
        public ICommand OnTriggerCommand { get; set; }
        public ICommand OnToggleCommand { get; set; }
        public ICommand OnExpandActionsCommand { get; set; }
        public ICommand OnCollapseActionsCommand { get; set; }

        public FunctoidChannelViewModel(IMessenger messenger, 
            IArduinoGateway arduinoGateway, 
            IFunctoidActionViewModelFactory actionFactory)
        {
            _messenger = messenger;
            _actionFactory = actionFactory;
            _arduinoGateway = arduinoGateway;

            OnDeleteCommand = new RelayCommand(OnDelete);
            OnAddActionCommand = new RelayCommand(OnAddAction);
            OnTriggerCommand = new RelayCommand(OnTrigger);
            OnToggleCommand = new RelayCommand(()  => this.IsEnabled = !this.IsEnabled);

            OnExpandActionsCommand = new RelayCommand(() => ToggleAction(true));
            OnCollapseActionsCommand = new RelayCommand(() => ToggleAction(false));

            Actions = new ObservableCollection<IFunctoidActionViewModel>();

            _messenger.Register<DeleteFunctoidActionMessage>(this, OnDeleteAction);
            _messenger.Register<PortConfigChangedMessage>(this, OnPortConfigChanged);
            _messenger.Register<ControllerStateMessage>(this, OnControllerStateChanged);

            AvailableInputs = new ObservableCollection<DeviceInputViewModel>();
        }

        private void OnControllerStateChanged(ControllerStateMessage obj)
        {
            if (this.SelectedInput == null) return;
            if (obj.Port != this.SelectedInput.Port.Position) return;
            foreach (var state in obj.States)
                if (state.Index == this.SelectedInput.Input.Position)
                {
                    this.Actions.Where(a => (int)a.Trigger == (int)state.State).ToList().ForEach(a => a.OnTriggerAction());
                    return;
                }
        }

        private void ToggleAction(bool isExpanded)
        {
            foreach (var action in this.Actions)
                action.IsExpanded = isExpanded;
        }

        private void OnPortConfigChanged(PortConfigChangedMessage obj)
        {
            RefreshInputs();
        }

        public void RefreshInputs()
        {
            AvailableInputs.Clear();
            if (_arduinoGateway == null) return;
            foreach (var port in _arduinoGateway.Ports)
            {
                if (port.ConnectedDevice == null) continue;

                foreach (var input in port.ConnectedDevice.GetInputs())
                {
                    AvailableInputs.Add(new DeviceInputViewModel(_messenger)
                    {
                        Device = port.ConnectedDevice,
                        Input = input,
                        Port = port,
                    });
                }
            }

            TryBindInput(_lastInput);
        }

        private void OnTrigger()
        {
            this.IsTriggered = true;
        }

        private void OnDeleteAction(DeleteFunctoidActionMessage obj)
        {
            Actions.Remove(obj.Item);
        }

        private void OnAddAction()
        {
            Actions.Add(_actionFactory.Build());
        }

        private async void OnDelete()
        {
            var result = await ConfirmationHelper.RequestConfirmationForDeletation();
            if (result is bool boolResult && boolResult)
            {
                _messenger.Send(new DeleteFunctoidChannelMessage(this));
            }
        }

        public void TryBindInput(string bindedInput)
        {
            this.SelectedInput = AvailableInputs.FirstOrDefault(d => d.FormatedName == bindedInput);
        }
    }
}