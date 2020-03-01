using System.Linq;
using MoonBurst.Core;

namespace MoonBurst.ViewModel
{
    public partial class HardwareConfigurationViewModelSerializer : SerializerBase<IHardwareConfigurationViewModel, HardwareConfigurationData>
    {
        public override string Default { get => "default_hardware.xml"; }

        public override HardwareConfigurationData ExtractData(IHardwareConfigurationViewModel source)
        {
            return new HardwareConfigurationData()
            {
                ComPort = source.SelectedComPort,
                MidiOut = source.SelectedOutputMidiDevice,
                Speed = source.SelectedSpeed,
                ArduinoPorts = source.ArduinoPorts.ToList().ConvertAll(c => c.GetData()).ToList()
            };
        }

        public override void ApplyData(HardwareConfigurationData config, IHardwareConfigurationViewModel target)
        {
            target.SelectedComPort = config.ComPort;
            target.SelectedOutputMidiDevice = config.MidiOut;
            target.SelectedSpeed = config.Speed;
            target.UpdateArduinoPorts(config.ArduinoPorts);
        }
    } 
}