using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    [Serializable]
    public class Bishop : BaseBishopCastleQueenKing
    {
        public override Kind Kind { get; set; } = Kind.Bishop;
        protected override int MaxSteps { get {return 7; } }

        protected override List<Tuple<int, int>> Directions {get;}  = new List<Tuple<int, int>>() {
                new Tuple<int,int>(1, 1),
                new Tuple<int,int>(-1, 1),
                new Tuple<int,int>(-1, -1),
                new Tuple<int,int>(1, -1),
            };


        public Bishop(Position position, Color color) : base(position, color)
        {
        }

        public override IPiece Clone()
        {
            var toReturn = new Bishop(new Position(Position.AsIndex), Color);

            return toReturn;
        }
    }
}
