using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Transactions;
using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    public enum Color
    {
        Black,
        White,
        None /* for empty houses */
    }

    public enum Kind
    {
        King = 'K',
        Queen = 'Q',
        Castle = 'R',
        Bishop = 'B',
        Knight = 'N',
        Pawn = 'P',
        Empty = ' '
    }

    public class Position
    {
        public string AsString { get; }
        public int AsIndex { get; }

        public Position(string fromString)
        {
            fromString = fromString.ToLower();
            var col = fromString[0]; // a, b, c, ... 
            var row = fromString.Substring(1, 1); // 1, 2, 3, ...

            int x = col - 'a'; // starting from 0
            int y = int.Parse(row) - 1; // starting from 0

            AsString = fromString;
            AsIndex = (8 * y) + x;
        }

        public Position(int fromIndex)
        {
            int x = fromIndex % 8; // starting from 0
            int y = fromIndex / 8; // starting from 0

            var col = ((char)('a' + x)).ToString();
            var row = (y + 1).ToString();

            AsString = col + row;
            AsIndex = fromIndex;
        }

        public Position MoveBy(int offsetY, int offsetX)
        {
            int currX = AsIndex % 8;
            int currY = AsIndex / 8;

            int destX = currX + offsetX;
            int destY = currY + offsetY;

            if (destX > 7 || destX < 0)
                return null;

            if (destY > 7 || destY < 0)
                return null;

            var newIndex = (8 * destY) + destX;

            return new Position(newIndex);

        }
    }

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

    public interface IPiece
    {
        Position Position { get; set; }
        Kind Kind { get; set; }
        Color Color { get; set; }

        BoardState Board { get; set; }
        List<Move> ValidMoves();
        void Move(string to, IPiece tookPiece = null);
        void Move(Position to, IPiece tookPiece = null);
        void Move(Move move);
        bool IsOpponentOf(IPiece other);
        bool IsEmptyHouse();
        Color OpponentsColor { get; }
    }

    public class BoardState
    {
        private Dictionary<Color, Action<Move>> _moveHandlers = new Dictionary<Color, Action<Move>>();

        public Color NextPlayer { get; set; } = Color.White;
        public IPiece[] Houses { get; set; }

        public BoardState()
        {
            _moveHandlers[Color.White] = (move) => {};
            _moveHandlers[Color.Black] = (move) => {};

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

        public IPiece PieceFactory(Kind kind, Color color, Position position)
        {
            switch (kind)
            {
                case Kind.Pawn: return new Pawn(position, color);
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
