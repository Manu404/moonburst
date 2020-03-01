using System.Linq;
using MoonBurst.Core;

namespace MoonBurst.ViewModel
{
    public partial class HardwareConfigurationViewModelSerializer : SerializerBase<HardwareConfigurationViewModel, HardwareConfigurationData>
    {
        public string Default { get => "default_hardware.xml"; }

        public override HardwareConfigurationData ExtractData(HardwareConfigurationViewModel source)
        {
            return new HardwareConfigurationData()
            {
                ComPort = source.SelectedComPort,
                MidiOut = source.SelectedOutputMidiDevice,
                Speed = source.SelectedSpeed,
                ArduinoPorts = source.ArduinoPorts.ToList().ConvertAll(c => c.GetData()).ToList()
            };
        }

        public override void ApplyData(HardwareConfigurationData config, HardwareConfigurationViewModel target)
        {
            target.SelectedComPort = config.ComPort;
            target.SelectedOutputMidiDevice = config.MidiOut;
            target.SelectedSpeed = config.Speed;
            target.UpdateArduinoPorts(config.ArduinoPorts);
        }
    } 
}