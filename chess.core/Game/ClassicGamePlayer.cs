using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    public enum Winner
    {
        White,
        Black,
        Draw
    }

    [Serializable]
    public class ClassicGamePlayer
    {
        public int numberOfMovesWithoutTakesForDraw { get; set; } = 150;
        public BoardState Board { get; }
        public Winner Winner { get; private set; }
        public ClassicGamePlayer()
        {
            Board = new BoardState();
            Board.InitClassicGame();
        }

        public IEnumerable<Move> Play()
        {
            var rnd = new Random();

            List<Move> moves = Board.GetMovesForPlayer(Board.NextPlayer);
            bool itsADraw = false;
            do
            {
                var numberOfMoves = moves.Count;
                var nextMoveIndex = rnd.Next(numberOfMoves - 1);
                var move = moves[nextMoveIndex];
                var toMove = Board.GetPieceAtPosition(move.From);
                toMove.Move(move);
                yield return move;

                moves = Board.GetMovesForPlayer(Board.NextPlayer);
                itsADraw = Board.MovesWithoutTakesCounter >= numberOfMovesWithoutTakesForDraw; 
                if (itsADraw)
                {
                    moves = new List<Move>();
                }
            }
            while (moves.Count > 0);

            Winner = itsADraw ? Winner.Draw : (Board.Opponent == Color.White ? Winner.White : Winner.Black);
        }
    }
}
