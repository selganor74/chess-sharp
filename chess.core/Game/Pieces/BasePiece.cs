using System.Collections.Generic;

namespace chess.core.Game
{

    public abstract class BasePiece : IPiece
    {
        public Position Position { get; set; }
        public abstract Kind Kind { get; set; }
        public Color Color { get; set; }
        public virtual BoardState Board { get; set; }

        public Color OpponentsColor => Color == Color.None ? Color.None : (Color == Color.White ? Color.Black : Color.White);

        public BasePiece(Position position, Color color)
        {
            Position = position;
            Color = color;
        }
        public abstract List<Move> ValidMoves();

        public virtual void Move(string to, IPiece tookPiece = null)
        {
            Move(new Position(to), tookPiece);
        }

        public virtual void Move(Position to, IPiece tookPiece = null)
        {
            var move = new Move() { Piece = this, From = this.Position, To = to, TookPiece = tookPiece };
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
    }
}