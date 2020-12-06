using System.Linq;
using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    [Serializable]
    public class ClassicGamePlayer
    {
        public BoardState Board { get; }
        public Color Winner {get; private set;}
        public ClassicGamePlayer()
        {
            Board = new BoardState();
            Board.InitClassicGame();
        }

        public IEnumerable<Move> Play() {
            var rnd = new Random();
            
            List<Move> moves = Board.GetMovesForPlayer(Board.NextPlayer);
            do
            {
                var numberOfMoves = moves.Count;
                var nextMoveIndex = rnd.Next(numberOfMoves - 1);
                var move = moves[nextMoveIndex];
                var toMove = Board.GetPieceAtPosition(move.From);
                toMove.Move(move);
                yield return move;

                moves = Board.GetMovesForPlayer(Board.NextPlayer);
                if (Board.MovesWithoutTakesCounter > 150) {
                    moves = new List<Move>();
                }
            }
            while(moves.Count > 0);

            Winner = Board.Opponent;
        }
    }
}
