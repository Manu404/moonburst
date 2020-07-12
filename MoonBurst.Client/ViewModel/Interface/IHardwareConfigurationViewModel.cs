using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoonBurst.Api.Client;
using MoonBurst.Api.Gateway.Midi;
using MoonBurst.Api.Gateway.Serial;
using MoonBurst.Api.Serializer;
using MoonBurst.Model;

namespace MoonBurst.ViewModel.Interface
{
    public interface IHardwareConfigurationViewModel : IFileSerializableType, IViewModel
    {
        ObservableCollection<IArduinoConfigPortViewModel> ArduinoPorts { get; }
        ComPort SelectedComPort { get; set; }
        MidiDevice SelectedOutputMidiDevice { get; set; }
        ComPortSpeed SelectedSpeed { get; set; }

        void Close();
        void LoadLastOrOptions();
        void UpdateArduinoPorts(List<ArduinoPortConfigModel> data);
        void UpdateCOMDevices(string selectedCom);
        string GetStatusString();

        event EventHandler ConfigurationChanged;
    }
}