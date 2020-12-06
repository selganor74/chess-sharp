using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    [Serializable]
    public class Castle : BaseBishopCastleQueenKing
    {
        public override Kind Kind { get; set; } = Kind.Castle;
        protected override int MaxSteps{ get {return 7; } }

        protected override List<Tuple<int, int>> Directions {get;}  = new List<Tuple<int, int>>() {
                new Tuple<int,int>( 0,  1),
                new Tuple<int,int>(-1,  0),
                new Tuple<int,int>( 0, -1),
                new Tuple<int,int>( 1,  0),
            };

        public Castle(Position position, Color color) : base(position, color)
        {
        }

        public override IPiece Clone()
        {
            var toReturn = new Castle(new Position(Position.AsIndex), Color);

            return toReturn;
        }
    }
}
