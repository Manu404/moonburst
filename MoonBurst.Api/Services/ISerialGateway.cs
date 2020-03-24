using MoonBurst.Api.Hardware;
using System.Collections.Generic;

namespace MoonBurst.Core
{
    public interface ISerialGateway
    {
        bool IsConnected { get; }
        int CurrentSpeed { get; set; }
        InputCOMPortData CurrentPort { get; set; }

        void Connect(IArduinoPort[] ports);
        void Close();
        List<InputCOMPortData> GetPorts();
        List<int> GetRates();
    }

    public class InputCOMPortData
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int MaxBaudRate { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is InputCOMPortData)) return false;
            return Id.Equals(((InputCOMPortData)obj).Id);
        }
    }
}