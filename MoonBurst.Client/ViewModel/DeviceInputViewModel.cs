using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Hardware;
using MoonBurst.Api.Parser;
using MoonBurst.Model.Messages;

namespace MoonBurst.ViewModel
{
    public class DeviceInputViewModel : ViewModelBase
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
        
        public DeviceInputViewModel(IMessenger messenger)
        {
            messenger.Register<ControllerStateMessage>(this, OnControllerStateChanged);
        }

        private void OnControllerStateChanged(ControllerStateMessage obj)
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