using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public IPiece GetOpponentAtPosition(Position position, IPiece piece)
        {
            var pieceAtDestination = GetPieceAtPosition(position);
            var toReturn = pieceAtDestination.IsOpponentOf(piece) ? pieceAtDestination : null;
            return toReturn;
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

        public List<Move> GetMovesForPlayer(Color playerColor) 
        {
            var moves = Houses.Where(p => p.Color == playerColor).SelectMany(e => e.ValidMoves()).ToList();

            return moves;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"/---------------------------------------\");
            for(int y = 7; y >= 0; y--) {
                var offset = 8 * y;
                sb.Append("|");
                var colorWhite = (char)27 + "[33m";
                var colorBlack = (char)27 + "[34m";
                var colorReset = (char)27 + "[37m";
                for(int x = 0; x < 8; x++) 
                {
                    string c = "";
                    switch(Houses[offset+x].Color) {
                        case Color.White: {c = colorWhite; break; }
                        case Color.Black: {c = colorBlack; break; }
                        case Color.None: {c = colorReset; break; }
                    }
                    sb.Append(@$"  {c}{((char)Houses[offset+x].Kind).ToString()}{colorReset} |");
                }
                sb.AppendLine();
                if(y != 0)
                    sb.AppendLine(@"|---------------------------------------|");
            }
            sb.AppendLine(@"\---------------------------------------/"); 

            return sb.ToString();           
        }
    }
}
