using System.Runtime.Serialization;
using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    [Serializable]
    public abstract class BasePiece : IPiece
    {
        public Position Position { get; set; }
        public abstract Kind Kind { get; set; }
        public Color Color { get; set; }
        public virtual BoardState Board {get;set;}

        public Color OpponentsColor {
            get {
                return Color == Color.None ? Color.None : (Color == Color.White ? Color.Black : Color.White);
            } 
        }

        public BasePiece(Position position, Color color)
        {
            Position = position;
            Color = color;
        }
        public abstract List<Move> ValidMoves();

        public virtual void Move(string to, MoveKind moveKind, IPiece tookPiece = null)
        {
            Move(new Position(to), moveKind, tookPiece);
        }

        public virtual void Move(Position to, MoveKind moveKind, IPiece tookPiece = null)
        {
            var move = new Move(moveKind, this, to, tookPiece);
            Move(move);
        }
        public virtual void Move(Move move)
        {
            Board.MakeMove(move);
        }

        public bool IsOpponentOf(IPiece other)
        {
            return !other.IsEmptyHouse() && other.Color != Color;
        }

        public bool IsEmptyHouse()
        {
            return false;
        }

        public abstract IPiece Clone();
    }
}