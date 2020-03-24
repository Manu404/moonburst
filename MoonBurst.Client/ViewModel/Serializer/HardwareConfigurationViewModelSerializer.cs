using System.Linq;
using MoonBurst.Core.Serializer;
using MoonBurst.Model.Serializable;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Serializer
{
    public class HardwareConfigurationViewModelSerializer : SerializerBase<IHardwareConfigurationViewModel, HardwareConfigurationData>
    {
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

        public HardwareConfigurationViewModelSerializer() : base("default_hardware.xml")
        {
        }
    } 
}