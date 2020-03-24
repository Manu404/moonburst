using NUnit.Framework;
using System;
using MoonBurst.Core.Helper;

namespace MoonBurst.Test
{
    [TestFixture]
    public class MusicalNoteHelperTester
    {
        [Test]
        public void If0GivenShouldReturnC0()
        {
            MusicalNoteHelper helper = new MusicalNoteHelper();
            Assert.AreEqual(helper.FromMidiValueToNoteName(0), "C-1");
        }

        [Test]
        public void If128GivenShouldThrowException()
        {
            MusicalNoteHelper helper = new MusicalNoteHelper();
            Assert.Throws<Exception>(() => helper.FromMidiValueToNoteName(128));
        }

        [Test]
        public void IfNegativeGivenShouldThrowException()
        {
            MusicalNoteHelper helper = new MusicalNoteHelper();
            Assert.Throws<Exception>(() => helper.FromMidiValueToNoteName(-1));
        }

        [Test]
        public void If127GivenShouldReturnG8()
        {
            MusicalNoteHelper helper = new MusicalNoteHelper();
            Assert.AreEqual(helper.FromMidiValueToNoteName(127), "G9");
        }
    }
}
