using System.Reflection;
using System;
using NUnit.Framework;
using chess.core.Game;

namespace chess.core.tests
{
    [TestFixture]
    public class TestsFor_Position
    {
        [Test]
        public void create_position_from_index() 
        {
            var p = new Position(0);
            Assert.AreEqual("a1", p.AsString);

            p = new Position(8);
            Assert.AreEqual("a2", p.AsString);

            p = new Position(63);
            Assert.AreEqual("h8", p.AsString);
        }

        public void create_position_from_string() 
        {
            var p = new Position("a1");
            Assert.AreEqual(0, p.AsIndex);

            p = new Position("a2");
            Assert.AreEqual(8, p.AsIndex);

            p = new Position("h8");
            Assert.AreEqual(63, p.AsIndex);
        }
    }
}
