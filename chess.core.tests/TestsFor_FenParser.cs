using System;
using chess.core.Game;
using NUnit.Framework;

namespace chess.core.tests
{
    [TestFixture]
    public class TestsFor_FenParser
    {
        [Test]
        public void it_must_be_possible_to_setup_a_board_from_a_fen_string()
        {
            var board = new BoardState();
            board.FromFenString("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Assert.IsTrue(board.NextPlayer == Color.White);
        }
    }
}
