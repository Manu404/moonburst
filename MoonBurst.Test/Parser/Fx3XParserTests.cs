using MooBurst.Parser.Fs3X;
using MoonBurst.Api.Hardware.Parser;
using NUnit.Framework;

namespace MoonBurst.Test
{
    [TestFixture]
    public class Fx3XParserTests : FootswtichParserTestBase
    {
        [Test]
        public void WhenPressingModeShouldCompleteSequence()
        {
            var toTest = new Fs3XParser();
            int index = 0;
            var bitSequence = new [] { 0, 1, 1, 0, 0 };
            for (int i = 0; i < bitSequence.Length; i++)
                Assert.AreEqual((toTest.ParseState(bitSequence[i], 0)[index] as IFootswitchState).States, GetSequence()[i]);
        }

        [Test]
        public void WhenPressingDownShouldCompleteSequence()
        {
            var toTest = new Fs3XParser();
            int index = 1;
            var bitSequence = new [] { 0, 2, 2, 0, 0 };
            for (int i = 0; i < bitSequence.Length; i++)
                Assert.AreEqual((toTest.ParseState(bitSequence[i], 0)[index] as IFootswitchState).States, GetSequence()[i]);
        }

        [Test]
        public void WhenPressingUpShouldCompleteSequence()
        {
            var toTest = new Fs3XParser();
            int index = 2;
            var bitSequence = new [] { 1, 0, 0, 1, 1 };
            for (int i = 0; i < bitSequence.Length; i++)
                Assert.AreEqual((toTest.ParseState(bitSequence[i], 0)[index] as IFootswitchState).States, GetSequence()[i]);
        }
    }
}