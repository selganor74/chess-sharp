using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    public class EmptyHouse : IPiece
    {
        public Position Position { get; set; }
        public Kind Kind { get; set; } = Kind.Empty;
        public Color Color { get; set; } = Color.None;
        public BoardState Board { get; set; }

        public Color OpponentsColor => Color.None;

        public EmptyHouse(Position position)
        {
            Position = position;
        }
        public List<Move> ValidMoves()
        {
            return new List<Move>();
        }
        public void Move(string to)
        {
            throw new Exception("Can't move an empty house!");
        }

        public void Move(Position to)
        {
            throw new Exception("Can't move an empty house!");
        }

        public bool IsOpponentOf(IPiece other)
        {
            return false;
        }

        public bool IsEmptyHouse()
        {
            return true;
        }

        public void Move(string to, IPiece tookPiece = null)
        {
            throw new Exception("Can't move an empty house!");
        }

        public void Move(Position to, IPiece tookPiece = null)
        {
            throw new Exception("Can't move an empty house!");
        }

        public void Move(Move move)
        {
            throw new NotImplementedException();
        }
    }
}