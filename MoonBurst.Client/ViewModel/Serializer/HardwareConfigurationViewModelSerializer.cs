using System.Linq;
using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel.Serializer
{
    public class HardwareConfigurationViewModelSerializer : SerializerBase<IHardwareConfigurationViewModel, HardwareConfigurationModel>
    {
        public override HardwareConfigurationModel ExtractData(IHardwareConfigurationViewModel source)
        {
            return new HardwareConfigurationModel()
            {
                ComPort = source.SelectedComPort,
                MidiOut = source.SelectedOutputMidiDevice,
                Speed = source.SelectedSpeed,
                ArduinoPorts = source.ArduinoPorts.ToList().ConvertAll(c => c.GetModel()).ToList()
            };
        }

        public override void ApplyData(HardwareConfigurationModel config, IHardwareConfigurationViewModel target)
        {
            target.SelectedOutputMidiDevice = config.MidiOut;
            target.SelectedSpeed = config.Speed;
            target.UpdateArduinoPorts(config.ArduinoPorts);
            target.UpdateCOMDevices(config.ComPort?.Name);
        }

        public HardwareConfigurationViewModelSerializer() : base("default_hardware.xml")
        {
        }
    } 
}