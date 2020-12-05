using System;
using System.Linq;
using chess.core.Game;
using NUnit.Framework;

namespace chess.core.tests
{
    [TestFixture]
    public class TestsFor_Knight
    {
        // These four values are the positions nearest to the border
        // in which a knight can make all the 8 moves
        [TestCase("c3")]
        [TestCase("f3")]
        [TestCase("f6")]
        [TestCase("c6")]
        public void a_knight_in_the_middle_of_the_chessboard_can_make_8_moves(string startPosition)
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;

            var whiteKnight = new Knight(new Position(startPosition), Color.White);
            board.PutPiece(whiteKnight);

            var moves = whiteKnight.ValidMoves();
            Assert.AreEqual(8, moves.Count);
        }

        [TestCase("a1")]
        [TestCase("h1")]
        [TestCase("h8")]
        [TestCase("a8")]
        public void a_knight_in_a_corner_can_make_2_moves(string startPosition)
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;

            var whiteKnight = new Knight(new Position(startPosition), Color.White);
            board.PutPiece(whiteKnight);

            var moves = whiteKnight.ValidMoves();
            Assert.AreEqual(2, moves.Count);
        }

        [Test]
        public void a_knight_can_take_an_opponent()
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;

            var whiteKnight = new Knight(new Position("b1"), Color.White);
            board.PutPiece(whiteKnight);

            var blackKnight = new Knight(new Position("c3"), Color.Black);
            board.PutPiece(blackKnight);

            var moves = whiteKnight.ValidMoves();
            Assert.IsNotNull(moves.SingleOrDefault(m => m.TookPiece == blackKnight));
        }

        [Test]
        public void a_knight_can_t_move_over_a_piece_of_same_coloe()
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;

            var whiteKnight = new Knight(new Position("b1"), Color.White);
            board.PutPiece(whiteKnight);

            var anotherWhiteKnight = new Knight(new Position("c3"), Color.White);
            board.PutPiece(anotherWhiteKnight);

            var moves = whiteKnight.ValidMoves();
            Assert.IsFalse(moves.Any(m => m.To.AsString == "c3"));
        }
    }
}
