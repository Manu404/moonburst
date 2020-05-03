using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Gateway.Arduino;
using MoonBurst.Api.Gateway.Serial;
using MoonBurst.Api.Hardware;
using MoonBurst.Api.Hardware.Description;
using MoonBurst.Api.Hardware.Parser;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{
    public class DeviceInputViewModel : ViewModelBase, IDeviceInputViewModel
    {
        private IFootswitchState _state;

        public IFootswitchState State
        {
            get => _state;
            set
            {
                _state = value;
                RaisePropertyChanged();
            }
        }

        public IArduinoPort Port { get; set; }
        public IDeviceDefinition Device { get; set; }
        public IDeviceInput Input { get; set; }
        
        public string PortName => $"Port: {Port.Position + 1}";
        public string DeviceName => $"Device: {Device.Name}";
        public string ControllerName => $"Controller: {ControllerNameWithoutHeader}";
        public string ControllerNameWithoutHeader => $"{Input.Name} ({Input.Position + 1})";
        public string FormatedName => $"{PortName}\n{DeviceName}\n{ControllerName}";
        
        public DeviceInputViewModel(ISerialGateway gateway)
        {
            gateway.OnTrigger += OnControllerStateChanged;
        }

        private void OnControllerStateChanged(object sender, ControllerStateEventArgs obj)
        {
            if (obj.Port != Port.Position) return;
            foreach(var state in obj.States)
                if (state.Index == Input.Position && state != State)
                {
                    State = state;
                    return;
                }
        }
    }
}