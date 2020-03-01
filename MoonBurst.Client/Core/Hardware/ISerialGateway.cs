using System.Collections.Generic;
using MoonBurst.Model;

namespace MoonBurst.Core
{
    public interface ISerialGateway
    {
        bool IsConnected { get; }
        int CurrentSpeed { get; set; }
        InputCOMPortData CurrentPort { get; set; }
        List<InputCOMPortData> GetPorts();
        List<int> GetRates();
        void Connect(IArduinoPort[] ports);
        void Close();
    }
}