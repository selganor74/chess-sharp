using System;

namespace chess.core.Game
{
    [Serializable]
    public class Move
    {
        public Color Player {get;}
        public Kind Piece { get;  }
        public Position From { get;  }
        public Position To { get;  }
        public Kind? TookPiece { get;  }
        public Position TookPiecePosition { get; }

        public Move(IPiece piece, Position to, Kind? tookPiece = null, Position tookPiecePosition = null)
        {
            Player = piece.Color;
            Piece = piece.Kind;
            From = piece.Position;
            To = to;
            TookPiece = tookPiece;
            TookPiecePosition = tookPiecePosition;
        }

        public Move(IPiece piece, Position to, IPiece tookPiece = null) : this(piece, to, tookPiece?.Kind, tookPiece?.Position)
        {
        }

        public Move(IPiece piece, string to, IPiece tookPiece = null) : this(piece, new Position(to), tookPiece)
        {
        }

        public override string ToString()
        {
            var take = TookPiece != null ? $"takes {TookPiece}" : "";
            return $"{Player.ToString()} {Piece.ToString()} ( {From.AsString} -> {To.AsString} ) {take}";
        }
    }
}
