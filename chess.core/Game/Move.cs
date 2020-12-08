using System;

namespace chess.core.Game
{
    public enum MoveKind {
        Move, 
        Take,
        Castling,
        TakeEnPassant,
        PawnPromotion,
    }
    [Serializable]
    public class Move
    {
        public MoveKind MoveKind {get;set;} = MoveKind.Move;
        public Color Player {get;}
        public Kind Piece { get;  }
        public Position From { get;  }
        public Position To { get;  }
        public Kind? TookPiece { get;  }
        public Position TookPiecePosition { get; }

        // Used only for Castling
        public Kind? CastlePiece { get;  } 
        public Position CastleFrom { get;  } = null;
        public Position CastleTo { get;  } = null;

        public Move(MoveKind moveKind, IPiece piece, Position to, Kind? tookPiece = null, Position tookPiecePosition = null)
        {
            MoveKind = moveKind;
            Player = piece.Color;
            Piece = piece.Kind;
            From = piece.Position;
            To = to;
            TookPiece = tookPiece;
            TookPiecePosition = tookPiecePosition;
        }

        public Move(MoveKind moveKind, IPiece piece, Position to, IPiece tookPiece = null) : this(moveKind, piece, to, tookPiece?.Kind, tookPiece?.Position)
        {
        }

        public Move(MoveKind moveKind, IPiece piece, Position to, Position castleFrom, Position castleTo) : this(moveKind, piece, to, null)
        {
            CastlePiece = Kind.Castle;
            CastleFrom = castleFrom;
            CastleTo = castleTo;
        }

        public Move(MoveKind moveKind, IPiece piece, string to, IPiece tookPiece = null) : this(moveKind, piece, new Position(to), tookPiece)
        {
        }

        public override string ToString()
        {
            var take = TookPiece != null ? $"takes {TookPiece}" : "";
            return $"{Player.ToString()} {Piece.ToString()} ( {From.AsString} -> {To.AsString} ) {take} {MoveKind.ToString()}";
        }
    }
}
