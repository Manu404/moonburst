using System;
using MoonBurst.Core.Helper;
using NUnit.Framework;

namespace MoonBurst.Test
{
    [TestFixture]
    public class LatinNotationFormatterTests
    {
        [Test]
        public void If0GivenShouldReturnCMinus1()
        {
            var helper = new LatinNotationFormatter();
            Assert.AreEqual(helper.GetName(0), "Do -1");
        }

        [Test]
        public void If128GivenShouldThrowException()
        {
            var helper = new LatinNotationFormatter();
            Assert.Throws<Exception>(() => helper.GetName(128));
        }

        [Test]
        public void IfNegativeGivenShouldThrowException()
        {
            var helper = new LatinNotationFormatter();
            Assert.Throws<Exception>(() => helper.GetName(-1));
        }

        [Test]
        public void If127GivenShouldReturnG9()
        {
            var helper = new LatinNotationFormatter();
            Assert.AreEqual(helper.GetName(127), "Sol 9");
        }
    }
}