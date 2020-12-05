using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    public class Queen : BaseBishopCastleQueenKing
    {
        public override Kind Kind { get; set; } = Kind.Queen;
        protected override int MaxSteps => 7;

        protected override List<Tuple<int, int>> Directions {get;}  = new List<Tuple<int, int>>() {
                new Tuple<int,int>( 1,  1),
                new Tuple<int,int>( 0,  1),
                new Tuple<int,int>(-1,  1),
                new Tuple<int,int>(-1,  0),
                new Tuple<int,int>(-1, -1),
                new Tuple<int,int>( 0, -1),
                new Tuple<int,int>( 1, -1),
                new Tuple<int,int>( 1,  0),
            };


        public Queen(Position position, Color color) : base(position, color)
        {
        }
    }
}
