using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    public class Bishop : BasePiece
    {
        public override Kind Kind { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Bishop(Position position, Color color) : base(position, color)
        {
        }

        public override List<Move> ValidMoves()
        {
            var toReturn = new List<Move>();

            var directions = new List<Tuple<int, int>>() {
                new Tuple<int,int>(1, 1),
                new Tuple<int,int>(-1, 1),
                new Tuple<int,int>(-1, -1),
                new Tuple<int,int>(1, -1),
            };

            foreach (var direction in directions)
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
                while(true);

            }

            return toReturn;
        }
    }
}
