using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    [Serializable]
    public abstract class BaseBishopCastleQueenKing : BasePiece
    {
        protected abstract List<Tuple<int,int>> Directions {get;}
        protected abstract int MaxSteps {get;}
        protected BaseBishopCastleQueenKing(Position position, Color color) : base(position, color)
        {
        }

        public override BoardState Board { get => base.Board; set => base.Board = value; }

        public override List<Move> ValidMoves()
        {
            var toReturn = new List<Move>();

            foreach (var direction in Directions)
            {
                var currentStep = 1;
                do
                {
                    var nextPosition = Position.MoveBy(direction.Item1 * currentStep, direction.Item2 * currentStep);
                    if (nextPosition == null)
                        break;

                    if (Board.IsPositionOccupiedByOpponent(nextPosition, this))
                    {
                        var takingMove = new Move(this, nextPosition, Board.GetOpponentAtPosition(nextPosition, this));
                        toReturn.Add(takingMove);
                        break;
                    }

                    if (!Board.IsPositionFree(nextPosition))
                    {
                        break;
                    }

                    var move = new Move(this, nextPosition, Board.GetOpponentAtPosition(nextPosition, this));
                    toReturn.Add(move);
                    currentStep++;
                } 
                while(currentStep <= MaxSteps );

            }

            return toReturn;
        }
    }
}
