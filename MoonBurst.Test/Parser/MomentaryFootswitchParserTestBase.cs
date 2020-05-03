using MoonBurst.Api.Hardware.Default;
using NUnit.Framework;

namespace MoonBurst.Test
{
    [TestFixture]
    public class MomentaryFootswitchParserTestBase : FootswtichParserTestBase
    {
        [Test]
        public void WhenPressingShouldCompleteSequence()
        {
            var toTest = new MomentaryFootswitchParser();
            var bitSequence = new int[] {0, 1, 1, 0, 0};
            for(int i = 0; i < bitSequence.Length; i++)
                Assert.AreEqual(toTest.ParseState(bitSequence[i], 0).States, GetSequence()[i]);
        }
    }
}