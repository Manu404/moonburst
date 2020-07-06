using MoonBurst.Api.Hardware.Default;
using MoonBurst.Api.Hardware.Description;
using MoonBurst.Api.Hardware.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonBurst.Parser.InC
{
    public class TC366Magnetophone : IDeviceDefinition
    {
        public IEnumerable<IDeviceInput> GetInputs()
        {
            return new List<IDeviceInput>
             {
                 new MomentaryFootswitchInput(0, "Play"),
                 new MomentaryFootswitchInput(1, "Stop")
             };
        }

        public string Name => "TC-366 magnetophone";
        public string Description => "Parser for the magnetophone used for In C";
        
        public IDeviceParser BuildParser()
        {
            return new GenericTwoMomentarySwitchStereoJack_Parser();
        }
    }
}
