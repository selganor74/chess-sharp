using System.Reflection;
using System;
using System.Linq;
using chess.core.Game;
using NUnit.Framework;

namespace chess.core.tests
{
    [TestFixture]
    public class TestsFor_Pawn
    {
        protected Pawn NewWhitePawn(string position) 
        {
            return new Pawn(new Position(position), Color.White);
        } 

        protected Pawn NewBlackPawn(string position) 
        {
            return new Pawn(new Position(position), Color.Black);
        } 

        [Test]
        public void a_border_pawn_on_its_first_move_must_have_2_valid_moves()
        {
            var board = new BoardState();
            var whitePawn = NewWhitePawn("a2");
            board.PutPiece(whitePawn);
            board.NextPlayer = Color.White;

            var validMoves = whitePawn.ValidMoves();
            Assert.AreEqual(2, validMoves.Count);

            var blackPawn = NewBlackPawn("a7");
            board.PutPiece(blackPawn);
            board.NextPlayer = Color.Black;

            validMoves = blackPawn.ValidMoves();
            Assert.AreEqual(2, validMoves.Count);
        }

        [Test]
        public void a_center_board_pawn_on_its_first_move_must_have_2_valid_moves()
        {
            var board = new BoardState();
            var whitePawn = NewWhitePawn("d2");
            board.PutPiece(whitePawn);
            board.NextPlayer = Color.White;

            var validMoves = whitePawn.ValidMoves();
            Assert.AreEqual(2, validMoves.Count);


            var blackPawn = NewBlackPawn("e7");
            board.PutPiece(blackPawn);
            board.NextPlayer = Color.White;

            validMoves = blackPawn.ValidMoves();
            Assert.AreEqual(2, validMoves.Count);
        }

        [Test]
        public void if_opponents_are_in_range_valid_moves_must_increase()
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;

            var whitePawn = NewWhitePawn("d2");
            board.PutPiece(whitePawn);
            
            // An opponent not in range
            var notInRangeblackPawn = NewBlackPawn("e7");
            board.PutPiece(notInRangeblackPawn);
            var validMoves = whitePawn.ValidMoves();
            Assert.AreEqual(2, validMoves.Count);

            // A NON-opponent in range
            var inRangeWhitePawn = NewWhitePawn("e3");
            board.PutPiece(inRangeWhitePawn);
            validMoves = whitePawn.ValidMoves();
            Assert.AreEqual(2, validMoves.Count);

            // An opponent in range
            var inRangeBlackPawn = NewBlackPawn("e3");
            board.PutPiece(inRangeBlackPawn);
            validMoves = whitePawn.ValidMoves();
            Assert.AreEqual(3, validMoves.Count);

            // Another opponent in range
            var anotherInRangeBlackPawn = NewBlackPawn("c3");
            board.PutPiece(anotherInRangeBlackPawn);
            validMoves = whitePawn.ValidMoves();
            Assert.AreEqual(4, validMoves.Count);
        }

        [Test]
        public void if_a_move_takes_an_opponent_the_move_should_report_it()
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;

            var whitePawn = NewWhitePawn("d2");
            board.PutPiece(whitePawn);
            
            // An opponent in range
            var inRangeBlackPawn = NewBlackPawn("e3");
            board.PutPiece(inRangeBlackPawn);
            
            var validMoves = whitePawn.ValidMoves();
            var aMoveThatTakes = validMoves.SingleOrDefault(m => m.TookPiece != null);
            Assert.IsNotNull(aMoveThatTakes);
            Assert.AreEqual(Kind.Pawn, aMoveThatTakes.TookPiece.Kind);
        }

        [Test]
        public void a_pawn_can_t_move_over_a_piece_of_same_color()
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;

            var whitePawn = NewWhitePawn("d2");
            board.PutPiece(whitePawn);
            
            var aPieceBlockingThe2StepsMove = NewWhitePawn("d4");
            board.PutPiece(aPieceBlockingThe2StepsMove);
            
            var validMoves = whitePawn.ValidMoves();
            Assert.AreEqual(1, validMoves.Count);

            var aPieceBlockingThe1StepMove = NewWhitePawn("d3");
            board.PutPiece(aPieceBlockingThe1StepMove);
            
            validMoves = whitePawn.ValidMoves();
            Assert.AreEqual(0, validMoves.Count);
        }

        [Test]
        public void a_first_move_pawn_can_be_taken_en_passant()
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;

            var whitePawn = NewWhitePawn("c2"); // this pawn will move its first move
            board.PutPiece(whitePawn);
            var blackPawn = NewBlackPawn("d4"); // this will take the previous "en passant" ( https://en.wikipedia.org/wiki/En_passant )
            board.PutPiece(blackPawn);

            whitePawn.Move("c4");
            var aMoveThatTakes = blackPawn.ValidMoves().SingleOrDefault(m => m.TookPiece == whitePawn);
            Assert.IsNotNull(aMoveThatTakes);
        }

        [Test]
        public void a_first_move_of_one_step_makes_the_pawn_immune_to_en_passant()
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;

            var whitePawn = NewWhitePawn("c2"); // this pawn will move its first move
            board.PutPiece(whitePawn);
            var blackPawn = NewBlackPawn("d3"); // this will not be able to take the previous "en passant" ( https://en.wikipedia.org/wiki/En_passant )
            board.PutPiece(blackPawn);

            whitePawn.Move("c3");
            var aMoveThatTakes = blackPawn.ValidMoves().SingleOrDefault(m => m.TookPiece == whitePawn);
            Assert.IsNull(aMoveThatTakes);
        }


        [Test]
        public void if_not_taken_en_passant_immediately_it_can_t_be_taken_any_more()
        {
            var board = new BoardState();
            board.NextPlayer = Color.White;

            var whitePawn = NewWhitePawn("c2"); // this pawn will move its first move
            board.PutPiece(whitePawn);

            var blackPawn = NewBlackPawn("d4"); // this will try to take the previous "en passant" ( https://en.wikipedia.org/wiki/En_passant )
            board.PutPiece(blackPawn);

            var neutralBlackPawn = NewBlackPawn("a7"); // this will be moved after
            board.PutPiece(neutralBlackPawn);
            
            whitePawn.Move("c4");
            neutralBlackPawn.Move("a6");
            board.NextPlayer = Color.Black;

            var aMoveThatTakes = blackPawn.ValidMoves().SingleOrDefault(m => m.TookPiece == whitePawn);
            Assert.IsNull(aMoveThatTakes);
        }
    }
}
