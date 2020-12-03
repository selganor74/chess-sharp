using System.Reflection;
using System;
using NUnit.Framework;
using chess.core.Game;

namespace chess.core.tests
{
    [TestFixture]
    public class TestsFor_Position
    {
        /* Here a conversion table from and to algebraic notation 
           of positions and array indexes (as stored in BoardState)
           eg.: d5 = 35
        /---------------------------------------\
    8   | 56 | 57 | 58 | 59 | 60 | 61 | 62 | 63 |
        |---------------------------------------|
    7   | 48 | 49 | 50 | 51 | 52 | 53 | 54 | 55 |
        |---------------------------------------|
    6   | 40 | 41 | 42 | 43 | 44 | 45 | 46 | 47 |
        |---------------------------------------|
    5   | 32 | 33 | 34 | 35 | 36 | 37 | 38 | 39 |
        |---------------------------------------|
    4   | 24 | 25 | 26 | 27 | 28 | 29 | 30 | 31 |
        |---------------------------------------|
    3   | 16 | 17 | 18 | 19 | 20 | 21 | 22 | 23 |
        |---------------------------------------|
    2   |  8 |  9 | 10 | 11 | 12 | 13 | 14 | 15 |
        |---------------------------------------|
    1   |  0 |  1 |  2 |  3 |  4 |  5 |  6 |  7 |
        \---------------------------------------/
           a    b    c    d    e    f    g    h
        */

        [TestCase(0,"a1")]
        [TestCase(8,"a2")]
        [TestCase(35,"d5")]
        [TestCase(63,"h8")]
        public void create_position_from_index(int index, string position) 
        {
            var p = new Position(index);
            Assert.AreEqual(position, p.AsString);
        }

        [TestCase(0,"a1")]
        [TestCase(8,"a2")]
        [TestCase(35,"d5")]
        [TestCase(63,"h8")]
        public void create_position_from_string(int index, string position) 
        {
            var p = new Position(position);
            Assert.AreEqual(index, p.AsIndex);
        }
    }
}
