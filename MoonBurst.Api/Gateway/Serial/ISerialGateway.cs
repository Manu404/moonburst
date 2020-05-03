using System;
using System.Collections.Generic;
using MoonBurst.Api.Gateway.Arduino;

namespace MoonBurst.Api.Gateway.Serial
{
    public interface ISerialGateway
    {
        bool IsConnected { get; }
        int CurrentSpeed { get; set; }
        ComPort CurrentPort { get; set; }

        void Connect(IArduinoPort[] ports);
        void Close();
        IEnumerable<ComPort> GetPorts();
        IEnumerable<int> GetRates();
        string GetStatusString();

        event EventHandler<SerialConnectionStateChangedEventArgs> ConnectionStateChanged;
        event EventHandler<ControllerStateEventArgs> OnTrigger;
    }
}