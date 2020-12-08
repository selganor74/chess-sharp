using System;
using System.Linq;
using chess.core.Game;
using NUnit.Framework;

namespace chess.core.tests
{
    [TestFixture]
    public class TestsFor_CheckMate
    {
        [Test]
        public void a_king_cannot_put_it_self_into_check() {
            var board = new BoardState();
            board.NextPlayer = Color.Black;

            var blackKing = new King(new Position("g3"), Color.Black);
            var whiteKing = new King(new Position("h1"), Color.White);
            board.PutPiece(blackKing);
            board.PutPiece(whiteKing);

            var moves = board.GetMovesForPlayer(Color.Black);
            Assert.IsFalse(moves.Any(m => m.To.AsIndex == (new Position("g2")).AsIndex));
        }
    }
}
