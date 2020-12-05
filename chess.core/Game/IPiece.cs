using System.Collections.Generic;

namespace chess.core.Game
{
    public interface IPiece
    {
        Position Position { get; set; }
        Kind Kind { get; set; }
        Color Color { get; set; }

        BoardState Board { get; set; }
        List<Move> ValidMoves();
        void Move(string to, IPiece tookPiece = null);
        void Move(Position to, IPiece tookPiece = null);
        void Move(Move move);
        bool IsOpponentOf(IPiece other);
        bool IsEmptyHouse();
        Color OpponentsColor { get; }
    }
}
