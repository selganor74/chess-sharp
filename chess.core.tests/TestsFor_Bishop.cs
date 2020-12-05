using System;
using System.Linq;
using chess.core.Game;
using NUnit.Framework;

namespace chess.core.tests
{
    [TestFixture]
    public class TestsFor_Bishop
    {
        // corners
        [TestCase("a1", 7)]
        [TestCase("h1", 7)]
        [TestCase("h8", 7)]
        [TestCase("a8", 7)]
        // middle of theboard
        [TestCase("d4", 13)]
        [TestCase("d5", 13)]
        [TestCase("e5", 13)]
        [TestCase("e4", 13)]
        public void number_of_valid_moves_relative_to_position(string position, int numberOfMoves)
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;
            
            var bishop = new Bishop(new Position(position), Color.White);
            board.PutPiece(bishop);
            var moves = bishop.ValidMoves();

            Assert.AreEqual(numberOfMoves, moves.Count);
        }

        [Test]
        public void bishop_movement_is_blocked_by_pieces_of_same_color()
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;
            
            var bishop = new Bishop(new Position("e4"), Color.White);
            board.PutPiece(bishop);
            var otherBishop1 = new Bishop(new Position("g6"), Color.White);
            board.PutPiece(otherBishop1);
            var otherBishop2 = new Bishop(new Position("c6"), Color.White);
            board.PutPiece(otherBishop2);
            var otherBishop3 = new Bishop(new Position("c2"), Color.White);
            board.PutPiece(otherBishop3);
            var otherBishop4 = new Bishop(new Position("g2"), Color.White);
            board.PutPiece(otherBishop4);

            var moves = bishop.ValidMoves();

            Assert.AreEqual(4, moves.Count);
        }

        [Test]
        public void bishop_movement_is_blocked_by_opponents_pieces()
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;
            
            var bishop = new Bishop(new Position("e4"), Color.White);
            board.PutPiece(bishop);
            var otherBishop1 = new Bishop(new Position("g6"), Color.Black);
            board.PutPiece(otherBishop1);
            var otherBishop2 = new Bishop(new Position("c6"), Color.Black);
            board.PutPiece(otherBishop2);
            var otherBishop3 = new Bishop(new Position("c2"), Color.Black);
            board.PutPiece(otherBishop3);
            var otherBishop4 = new Bishop(new Position("g2"), Color.Black);
            board.PutPiece(otherBishop4);

            var moves = bishop.ValidMoves();

            Assert.AreEqual(8, moves.Count);
            Assert.AreEqual(4, moves.Count(m => m.TookPiece != null));
        }
    }
}
