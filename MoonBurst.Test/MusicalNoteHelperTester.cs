using NUnit.Framework;
using System;
using MoonBurst.Core.Helper;

namespace MoonBurst.Test
{
    [TestFixture]
    public class EnglishNotationFormatterTests
    {
        [Test]
        public void If0GivenShouldReturnCMinus1()
        {
            var helper = new EnglishNotationFormatter();
            Assert.AreEqual(helper.GetPitchName(0), "C-1");
        }

        [Test]
        public void If128GivenShouldThrowException()
        {
            var helper = new EnglishNotationFormatter();
            Assert.Throws<Exception>(() => helper.GetPitchName(128));
        }

        [Test]
        public void IfNegativeGivenShouldThrowException()
        {
            var helper = new EnglishNotationFormatter();
            Assert.Throws<Exception>(() => helper.GetPitchName(-1));
        }

        [Test]
        public void If127GivenShouldReturnG9()
        {
            var helper = new EnglishNotationFormatter();
            Assert.AreEqual(helper.GetPitchName(127), "G9");
        }
    }
}
