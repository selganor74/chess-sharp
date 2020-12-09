using System;

namespace chess.core.Game.Moves
{
    public class TakeMove : Move
    {
        public override MoveKind MoveKind {get;} = MoveKind.Take;
        public TakeMove(IPiece piece, Position to, IPiece tookPiece)
        {
            Player = piece.Color;
            Piece = piece.Kind;
            From = piece.Position;
            To = to;
            TookPiece = tookPiece.Kind;
            TookPiecePosition = tookPiece.Position;
        }
    }
}
