using System.Net.NetworkInformation;
using System;

namespace chess.core.Game.Moves
{
    public class SimpleMove : Move
    {
        public override MoveKind MoveKind {get;} = MoveKind.Move;
        public SimpleMove(IPiece piece, Position to)
        {
            Player = piece.Color;
            Piece = piece.Kind;
            From = piece.Position;
            To = to;
        }
    }
}
