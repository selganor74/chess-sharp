using System;

namespace chess.core.Game
{
    public enum MoveKind
    {
        Move,
        Take,
        Castling,
        TakeEnPassant,
        PawnPromotion,
    }
    [Serializable]
    public abstract class Move
    {
        public abstract MoveKind MoveKind { get; }
        public Color Player { get; protected set; }
        public Kind Piece { get; protected set; }
        public Position From { get; protected set; }
        public Position To { get; protected set; }
        public Kind? TookPiece { get; protected set; }
        public Position TookPiecePosition { get; protected set; }

        // Used only for Castling
        public Kind? CastlePiece { get; protected set;}
        public Position CastleFrom { get; protected set;} = null;
        public Position CastleTo { get; protected set; } = null;


        public override string ToString()
        {
            var take = TookPiece != null ? $"takes {TookPiece}" : "";
            return $"{Player.ToString()} [{MoveKind.ToString()}] {Piece.ToString()} ( {From.AsString} -> {To.AsString} ) {take}";
        }
    }
}
