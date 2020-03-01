using MoonBurst.Core;
using MoonBurst.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MoonBurst.ViewModel
{
    public interface IHardwareConfigurationViewModel : IFileSerializableType, IViewModel
    {
        ObservableCollection<ArduinoConfigPortViewModel> ArduinoPorts { get; }
        InputCOMPortData SelectedComPort { get; set; }
        OutputMidiDeviceData SelectedOutputMidiDevice { get; set; }
        int SelectedSpeed { get; set; }

        void Close();
        void LoadLastConfig();
        void UpdateArduinoPorts(List<ArduinoPortConfigData> data);
    }
}