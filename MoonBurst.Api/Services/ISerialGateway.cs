using System;
using System.Collections.Generic;
using MoonBurst.Api.Hardware;
using MoonBurst.Api.Parser;

namespace MoonBurst.Api.Services
{
    public interface ISerialGateway
    {
        bool IsConnected { get; }
        int CurrentSpeed { get; set; }
        InputComPortData CurrentPort { get; set; }

        void Connect(IArduinoPort[] ports);
        void Close();
        IEnumerable<InputComPortData> GetPorts();
        IEnumerable<int> GetRates();

        event EventHandler<SerialConnectionStateChangedEventArgs> ConnectionStateChanged;
        event EventHandler<ControllerStateEventArgs> OnTrigger;
    }
    
    public class ControllerStateEventArgs : EventArgs
    {
        public ControllerStateEventArgs(MomentaryFootswitchState[] states, int port)
        {
            States = states;
            Port = port;
        }

        public MomentaryFootswitchState[] States { get; }
        public int Port { get; }
    }
    
    public class SerialConnectionStateChangedEventArgs : EventArgs
    {
        public SerialConnectionStateChangedEventArgs(bool newState)
        {
            NewState = newState;
        }

        public bool NewState { get; }
    }

    public class InputComPortData
    {
        public InputComPortData()
        {
            
        }
        
        public InputComPortData(string id)
        {
            Id = id;
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public int MaxBaudRate { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is InputComPortData)) return false;
            return Id.Equals(((InputComPortData)obj).Id);
        }

        protected bool Equals(InputComPortData other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}