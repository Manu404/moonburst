using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        int SelectedSpeed { get; set; }

        void Close();
        void LoadLast();
        void UpdateArduinoPorts(List<ArduinoPortConfigModel> data);
        string GetStatusString();
    }
}