using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoonBurst.Api.Services;
using MoonBurst.Core;
using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.Model.Serializable;

namespace MoonBurst.ViewModel.Interfaces
{
    public interface IHardwareConfigurationViewModel : IFileSerializableType, IViewModel
    {
        ObservableCollection<ArduinoConfigPortViewModel> ArduinoPorts { get; }
        InputComPortData SelectedComPort { get; set; }
        OutputMidiDeviceData SelectedOutputMidiDevice { get; set; }
        int SelectedSpeed { get; set; }

        void Close();
        void LoadLastConfig();
        void UpdateArduinoPorts(List<ArduinoPortConfigData> data);
    }
}