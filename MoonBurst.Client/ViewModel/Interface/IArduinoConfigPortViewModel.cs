using System.Collections.ObjectModel;
using System.Windows.Input;
using MoonBurst.Api.Gateway.Arduino;
using MoonBurst.Api.Hardware.Description;
using MoonBurst.Model;

namespace MoonBurst.ViewModel.Interface
{
    public interface IArduinoConfigPortViewModel
    {
        IDeviceDefinition ConnectedDevice { get; set; }
        bool IsEnabled { get; set; }
        bool IsConnected { get; set; }
        IArduinoPort Port { get; }
        int Position { get; }
        ObservableCollection<IDeviceDefinition> AvailableDevices { get; }
        ObservableCollection<IDeviceInputViewModel> AvailableInputs { get; }
        string PortName { get; }
        string ConnectedDeviceName { get; }
        ICommand ConnectCommand { get; }
        ICommand DisableCommand { get; }
        void ToggleEnable();
        void Connect(string obj);
        void RefreshInputs();
        ArduinoPortConfigModel GetModel();
    }
}