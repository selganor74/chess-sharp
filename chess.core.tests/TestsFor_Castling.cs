using System;
using System.Linq;
using chess.core.Game;
using NUnit.Framework;

namespace chess.core.tests
{
    [TestFixture]
    public class TestsFor_Castling
    {
        [Test]
        public void check_castling_on_the_left() {
            var board = new BoardState();
            var king = new King(new Position("e1"), Color.White);
            var castle = new Castle(new Position("a1"), Color.White);
            board.PutPiece(king);
            board.PutPiece(castle);

            var validMoves = king.ValidMoves();
            Assert.IsTrue(validMoves.Any(m => m.MoveKind == MoveKind.Castling));

            var move = validMoves.Single(m => m.MoveKind == MoveKind.Castling);
            king.Move(move);
            Assert.AreEqual(Kind.King, board.GetPieceAtPosition(new Position("c1")).Kind);
            Assert.AreEqual(Kind.Castle, board.GetPieceAtPosition(new Position("d1")).Kind);
        }

        [Test]
        public void check_castling_on_the_right() {
            var board = new BoardState();
            var king = new King(new Position("e1"), Color.White);
            var castle = new Castle(new Position("h1"), Color.White);
            board.PutPiece(king);
            board.PutPiece(castle);

            var validMoves = king.ValidMoves();
            Assert.IsTrue(validMoves.Any(m => m.MoveKind == MoveKind.Castling));

            var move = validMoves.Single(m => m.MoveKind == MoveKind.Castling);
            king.Move(move);
            Assert.AreEqual(Kind.King, board.GetPieceAtPosition(new Position("g1")).Kind);
            Assert.AreEqual(Kind.Castle, board.GetPieceAtPosition(new Position("f1")).Kind);
        }
    }
}
