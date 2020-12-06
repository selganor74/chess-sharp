using System;

namespace chess.core.Game
{
    [Serializable]
    public class Move
    {
        public IPiece Piece { get; set; }
        public Position From { get; set; }
        public Position To { get; set; }
        public IPiece TookPiece { get; set; }

        public Move() { }
        public Move(IPiece piece, Position to, IPiece tookPiece = null)
        {
            Piece = piece;
            From = piece.Position;
            To = to;
            TookPiece = tookPiece;
        }

        public Move(IPiece piece, string to)
        {
            Piece = piece;
            From = piece.Position;
            To = new Position(to);
        }

        public override string ToString()
        {
            var take = TookPiece != null ? "takes " + TookPiece.Kind.ToString() : "";
            return $"{Piece.Color.ToString()} {Piece.Kind.ToString()} ( {From.AsString} -> {To.AsString} ) {take}";
        }
    }
}
