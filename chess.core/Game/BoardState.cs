using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace chess.core.Game
{
    [Serializable]
    public class BoardState
    {
        private Dictionary<Color, Action<Move>> _moveHandlers = new Dictionary<Color, Action<Move>>();

        public Color NextPlayer { get; set; } = Color.White;
        public Color Opponent { get { return NextPlayer == Color.White ? Color.Black : Color.White; } }
        public bool PlayerIsUnderCheck {get { return GetMovesForPlayer(Opponent, false).Any(m => m.TookPiece?.Kind == Kind.King); } }
        public IPiece[] Houses { get; set; }
        public int MovesCounter { get; private set; }
        public int MovesWithoutTakesCounter { get; private set; }
        public Move LastMove { get; private set; }

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
            move = new Move()
            {
                From = new Position(move.From.AsIndex),
                To = new Position(move.To.AsIndex),
                Piece = GetPieceAtPosition(move.From),
                TookPiece = move.TookPiece == null ? null : GetPieceAtPosition(move.TookPiece.Position)
            };

            var source = Houses[move.From.AsIndex];
            if (source.Kind != move.Piece.Kind || source.Position.AsIndex != move.From.AsIndex)
                throw new Exception($"Invalid move. Piece at {move.From.AsString} is not of kind {move.Piece.Kind.ToString()}, but is a {source.Kind.ToString()}");

            if (NextPlayer != move.Piece.Color)
                throw new Exception($"It's {NextPlayer.ToString()} turn! Tried to move {move.Piece.Color} piece");

            var pieceAtSource = GetPieceAtPosition(move.From);
            move.Piece = pieceAtSource;
            var pieceAtDestination = GetPieceAtPosition(move.To);

            if (pieceAtDestination.IsOpponentOf(move.Piece))
                move.TookPiece = pieceAtDestination;

            RemovePieceAt(move.From);
            PutPieceAt(move.Piece, move.To);

            if(move.Piece is Pawn && move.To.Row == ((move.Piece.Color == Color.Black) ? 0 : 7)) 
            {
                var queen = new Queen(move.To, move.Piece.Color);
                RemovePieceAt(move.To);
                PutPiece(queen);
            }

            NextPlayer = move.Piece.OpponentsColor;
            MovesCounter++;
            LastMove = move;
            if (move.TookPiece != null)
                MovesWithoutTakesCounter = 0;
            else
                MovesWithoutTakesCounter++;

            _moveHandlers[move.Piece.Color]?.Invoke(move);
        }

        public List<Move> GetMovesForPlayer(Color playerColor, bool checkForChecks = true)
        {
            var moves = Houses.Where(p => p.Color == playerColor).SelectMany(e => e.ValidMoves()).ToList();

            if (checkForChecks)
            {
                // Moves that not cause a Check
                foreach (var m in moves.ToList())
                {
                    var simulated = SimulateMove(m);
                    var opponentsMoves = simulated.GetMovesForPlayer(playerColor == Color.White ? Color.Black : Color.White, false);
                    var causesCheck = opponentsMoves.Any(om => om.TookPiece?.Kind == Kind.King);
                    if (causesCheck)
                        moves.Remove(m);
                }
            }

            return moves;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var isUnderCheck = PlayerIsUnderCheck ? "[ Check! ]" : "";
            sb.AppendLine($"Move: {MovesCounter}/{MovesWithoutTakesCounter} - Next Player: {NextPlayer.ToString()} {isUnderCheck} - Last Move: {LastMove?.ToString()}");
            sb.AppendLine(@"  /---------------------------------------\");
            for (int y = 7; y >= 0; y--)
            {
                var offset = 8 * y;
                sb.Append($"{ y + 1 } |");
                var colorWhite = (char)27 + "[33m"; // https://en.wikipedia.org/wiki/ANSI_escape_code#SGR
                var colorBlack = (char)27 + "[34m";
                var colorReset = (char)27 + "[37m";
                for (int x = 0; x < 8; x++)
                {
                    string c = "";
                    switch (Houses[offset + x].Color)
                    {
                        case Color.White: { c = colorWhite; break; }
                        case Color.Black: { c = colorBlack; break; }
                        case Color.None: { c = colorReset; break; }
                    }
                    sb.Append(@$"  {c}{((char)Houses[offset + x].Kind).ToString()}{colorReset} |");
                }
                sb.AppendLine();
                if (y != 0)
                    sb.AppendLine(@"  |---------------------------------------|");
            }
            sb.AppendLine(@"  \---------------------------------------/");
            sb.AppendLine(@"     a    b    c    d    e    f    g    h  ");

            return sb.ToString();
        }

        protected BoardState SimulateMove(Move move)
        {
            var clonedBoard = this.CloneBoard();
            clonedBoard.MakeMove(move);
            return clonedBoard;
        }

        protected BoardState CloneBoard()
        {
            var toReturn = new BoardState()
            {
                // LastMove = LastMove != null ? new Move()
                // {
                //     From = new Position(LastMove.From.AsIndex),
                //     To = new Position(LastMove.To.AsIndex),
                //     Piece = LastMove.Piece.Clone(),
                //     TookPiece = LastMove?.TookPiece?.Clone()
                // } : null,
                NextPlayer = this.NextPlayer,
                MovesCounter = this.MovesCounter
            };

            foreach (var h in Houses)
            {
                var cloned = h.Clone();
                toReturn.PutPiece(cloned);
            }

            // Console.WriteLine(toReturn.ToString());
            return toReturn;
        }

        protected T DeepClone<T>(T toClone)
        {
            if (toClone == null)
                return toClone;

            using (var ms = new MemoryStream())
            {
                try
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, toClone);
                    ms.Position = 0;

                    return (T)formatter.Deserialize(ms);
                }
                catch
                {
                    Console.WriteLine(toClone.GetType().Name);
                    return toClone;
                }
            }
        }
    }
}
