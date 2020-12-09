using System.Net.NetworkInformation;
using System;

namespace chess.core.Game.Moves
{
    public class CastlingMove : Move
    {
        public override MoveKind MoveKind {get;} = MoveKind.Castling;
        public CastlingMove(IPiece king, Position kingDestination, IPiece castle, Position castleDestination )
        {
            Player = king.Color;
            Piece = king.Kind;
            From = king.Position;
            To = kingDestination;
            CastleFrom = castle.Position;
            CastleTo = castleDestination;
        }
    }
}
