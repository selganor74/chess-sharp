namespace chess.core.Game
{
    public class Move
    {
        public IPiece Piece { get; set; }
        public Position From { get; set; }
        public Position To { get; set; }
        public IPiece TookPiece { get; set; }

        public Move() { }
        public Move(IPiece piece, Position to)
        {
            Piece = piece;
            From = piece.Position;
            To = to;
        }

        public Move(IPiece piece, string to)
        {
            Piece = piece;
            From = piece.Position;
            To = new Position(to);
        }
    }
}
