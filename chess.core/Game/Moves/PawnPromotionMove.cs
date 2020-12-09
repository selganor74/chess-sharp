using System;

namespace chess.core.Game.Moves
{
    public class PawnPromotionMove : Move
    {
        public override MoveKind MoveKind {get;} = MoveKind.PawnPromotion;
        public PawnPromotionMove(IPiece piece, Position to)
        {
            Player = piece.Color;
            Piece = piece.Kind;
            From = piece.Position;
            To = to;
        }
    }
}
