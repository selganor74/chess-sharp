using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    public class Bishop : BaseBishopCastleQueenKing
    {
        public override Kind Kind { get; set; } = Kind.Bishop;
        protected override int MaxSteps => 7;

        protected override List<Tuple<int, int>> Directions {get;}  = new List<Tuple<int, int>>() {
                new Tuple<int,int>(1, 1),
                new Tuple<int,int>(-1, 1),
                new Tuple<int,int>(-1, -1),
                new Tuple<int,int>(1, -1),
            };


        public Bishop(Position position, Color color) : base(position, color)
        {
        }

    }
}
