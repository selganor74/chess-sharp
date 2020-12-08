using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    [Serializable]
    public class King : BaseBishopCastleQueenKing
    {
        public override Kind Kind {get;set;} = Kind.King;
        protected override int MaxSteps { get {return 1; } }

        protected override List<Tuple<int, int>> Directions {get;} = new List<Tuple<int, int>>() {
                new Tuple<int,int>( 1,  1),
                new Tuple<int,int>( 0,  1),
                new Tuple<int,int>(-1,  1),
                new Tuple<int,int>(-1,  0),
                new Tuple<int,int>(-1, -1),
                new Tuple<int,int>( 0, -1),
                new Tuple<int,int>( 1, -1),
                new Tuple<int,int>( 1,  0),
            };

        public King(Position position, Color color) : base(position, color)
        {
        }

        public override IPiece Clone()
        {
            var toReturn = new King(new Position(Position.AsIndex), Color);

            return toReturn;
        }
    }
}
