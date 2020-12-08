using System.Collections.Generic;

namespace chess.core.Game
{
    public interface IPiece
    {
        Position Position { get; set; }
        Kind Kind { get; set; }
        Color Color { get; set; }
        Color OpponentsColor { get; }

        BoardState Board { get; set; }
        List<Move> ValidMoves();
        void Move(string to, MoveKind moveKind, IPiece tookPiece = null);
        void Move(Position to, MoveKind moveKind, IPiece tookPiece = null);
        void Move(Move move);
        bool IsOpponentOf(IPiece other);
        bool IsEmptyHouse();

        // Creates a perfecto copy of the Piece, 
        //excluding so the piece must be replaced into the board.
        IPiece Clone();
    }
}
