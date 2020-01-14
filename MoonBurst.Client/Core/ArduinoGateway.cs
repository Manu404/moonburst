using System.Collections.Generic;
using System.Linq;
using MoonBurst.Model;

namespace MoonBurst.Core
{
    public interface IDeviceInput
    {
        int Position { get; }
        string Name { get; }
    }

    public interface IBooleanInput : IDeviceInput
    {

    }

    public interface INumericInput : IDeviceInput
    {

    }

    public class MomentaryFootswitchInput : IBooleanInput
    {
        public int Position { get; }
        public string Name { get; }

        public MomentaryFootswitchInput(int position, string name)
        {
            Position = position;
            Name = name;
        }
    }

    public interface IDeviceDefinition
    {
        IList<IDeviceInput> GetInputs();
        string Name { get; }
        string Description { get; }
    }

    public class Fs3xDeviceDefinition : IDeviceDefinition
    {
        public IList<IDeviceInput> GetInputs()
        {
            return new List<IDeviceInput>()
            {
                new MomentaryFootswitchInput(0, "Mode"),
                new MomentaryFootswitchInput(1, "Down"),
                new MomentaryFootswitchInput(2, "Up"),
            };
        }

        public string Name => "FS3X Footswitch";
        public string Description => "Digitech FS3X Footswitch device";
    }

    public class GenericOneMomentarySwitchMonoJackDefinition : IDeviceDefinition
    {
        public IList<IDeviceInput> GetInputs()
        {
            return new List<IDeviceInput>()
            {
                new MomentaryFootswitchInput(0,  "Switch")
            };
        }

        public string Name => "Generic Momentary Switch";
        public string Description => "Generic Momentary Switch Mono Connector";
    }

    public interface IArduinoPort
    {
        int Position { get; }
        IDeviceDefinition ConnectedDevice { get; set; }
        IList<IDeviceDefinition> AvailableDevices { get; }
    }

    public class ArduinoPort : IArduinoPort
    {
        public int Position { get; }
        public IDeviceDefinition ConnectedDevice { get; set; }
        public IList<IDeviceDefinition> AvailableDevices { get; }

        public ArduinoPort(int index)
        {
            Position = index;
            AvailableDevices = new List<IDeviceDefinition>();
            AvailableDevices.Add(new Fs3xDeviceDefinition());
        }
    }

    public interface IArduinoGateway
    {
        IArduinoPort[] Ports { get; }
        List<IDeviceDefinition> GetConnectedDevices();
    }

    public class ArduinoGateway : IArduinoGateway
    {
        public IArduinoPort[] Ports { get; }

        public List<IDeviceDefinition> GetConnectedDevices()
        {
            return Ports.Select(p => p.ConnectedDevice).ToList();
        } 

        public ArduinoGateway()
        {
            int portCount = 4;
            Ports = new IArduinoPort[portCount];
            InitializePorts();
        }

        private void InitializePorts()
        {
            for (int i = 0; i < Ports.Length; i++)
            {
                Ports[i] = new ArduinoPort(i);
            }
        }
    }
}