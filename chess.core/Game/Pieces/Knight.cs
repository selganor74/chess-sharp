using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Security.Authentication.ExtendedProtection;
using System.Reflection.Metadata;
using System.Globalization;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using chess.core.Game.Moves;

namespace chess.core.Game
{
    [Serializable]
    public class Knight : BasePiece
    {
        public override Kind Kind { get; set; } = Kind.Knight;

        public Knight(Position position, Color color) : base(position, color)
        {

        }

        public override List<Move> ValidMoves()
        {
            var toReturn = new List<Move>();
            var moves = new Tuple<int, int>[] {
                new Tuple<int,int>(-1,2),
                new Tuple<int,int>(1,2),
                new Tuple<int,int>(2,1),
                new Tuple<int,int>(2,-1),
                new Tuple<int,int>(1,-2),
                new Tuple<int,int>(-1,-2),
                new Tuple<int,int>(-2,-1),
                new Tuple<int,int>(-2,1)
            };

            foreach (var m in moves)
            {
                var destination = this.Position.MoveBy(m.Item1, m.Item2);
                if (destination == null) 
                    continue;

                if (Board.IsPositionFree(destination) || Board.IsPositionOccupiedByOpponent(destination, this))
                {
                    var eventualOpponent = Board.GetOpponentAtPosition(destination, this);
                    Move mv = null;
                    if (eventualOpponent != null)
                        mv = new TakeMove(this, destination, eventualOpponent);
                    else
                        mv = new SimpleMove(this, destination);

                    toReturn.Add(mv);
                }

            }

            return toReturn;
        }

        public override IPiece Clone()
        {
            var toReturn = new Knight(new Position(Position.AsIndex), Color);

            return toReturn;
        }
    }
}
