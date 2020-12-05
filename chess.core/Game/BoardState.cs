using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    public class BoardState
    {
        private Dictionary<Color, Action<Move>> _moveHandlers = new Dictionary<Color, Action<Move>>();

        public Color NextPlayer { get; set; } = Color.White;
        public IPiece[] Houses { get; set; }

        public BoardState()
        {
            _moveHandlers[Color.White] = (move) => { };
            _moveHandlers[Color.Black] = (move) => { };

            Houses = new IPiece[64];
            for (int i = 0; i < 64; i++)
                Houses[i] = new EmptyHouse(new Position(i));
        }

        public void RemovePieceAt(Position position)
        {
            GetPieceAtPosition(position).Board = null;
            PutPieceAt(new EmptyHouse(position), position);
        }

        public void RegisterPlayerMoveHandler(Color player, Action<Move> handler)
        {
            _moveHandlers[player] += handler;
        }

        public void UnregisterPlayerMoveHandler(Color player, Action<Move> handler)
        {
            _moveHandlers[player] -= handler;
        }

        public void PutPieceAt(IPiece piece, Position position)
        {
            piece.Position = position;
            piece.Board = this;
            Houses[piece.Position.AsIndex] = piece;
        }

        public void PutPiece(IPiece piece)
        {
            PutPieceAt(piece, piece.Position);
        }

        public IPiece GetPieceAtPosition(Position position)
        {
            return Houses[position.AsIndex];
        }

        public bool IsPositionFree(Position position)
        {
            return GetPieceAtPosition(position).IsEmptyHouse();
        }

        public bool IsPositionOccupiedByOpponent(Position position, IPiece piece)
        {
            var pieceAtPosition = GetPieceAtPosition(position);
            
            if (pieceAtPosition.IsEmptyHouse()) 
                return false;
            
            return pieceAtPosition.IsOpponentOf(piece);
        }

        public IPiece PieceFactory(Kind kind, Color color, Position position)
        {
            switch (kind)
            {
                case Kind.Pawn: return new Pawn(position, color);
                case Kind.Knight: return new Knight(position, color);
            }
            throw new Exception($"Can't create piece of kind {kind.ToString()}");
        }

        public void InitClassicGame()
        {

        }


        public void MakeMove(Move move)
        {
            var source = Houses[move.From.AsIndex];
            if (source != move.Piece)
                throw new Exception($"Invalid move. Piece at {move.From.AsString} is not of kind ${move.Piece.Kind.ToString()}, but is a {source.Kind.ToString()}");

            if (NextPlayer != move.Piece.Color)
                throw new Exception($"It's {NextPlayer.ToString()} turn! Tried to move {move.Piece.Color} piece");

            var pieceAtDestination = GetPieceAtPosition(move.To);

            if (pieceAtDestination.IsOpponentOf(move.Piece))
                move.TookPiece = pieceAtDestination;

            RemovePieceAt(move.From);
            PutPieceAt(move.Piece, move.To);

            NextPlayer = move.Piece.OpponentsColor;
            _moveHandlers[move.Piece.Color]?.Invoke(move);
        }
    }
}
