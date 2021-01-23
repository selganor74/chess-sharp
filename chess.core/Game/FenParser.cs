using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    internal class FenString
    {
        public string PiecePlacement { get; set; } = "";
        public string ActiveColor {get;set;} = "";
        
        public string CastlingAvailability {get;set;} = "-";

        public string EnPassantTargetSquare {get;set;} = "-";
        public string HalfMovesClock {get;set;} = "0";
        public string FullMoves {get;set;} = "0";
        public FenString(string fenString)
        {
            var fenParts = fenString.Split(' ');
            var numOfFenElements = fenParts.Length;
            if (numOfFenElements < 2)
                throw new Exception($"{nameof(FenString)} needs at least board state and next player information. Please check https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation for more information.");

            PiecePlacement = fenParts[0];
            ActiveColor = fenParts[1];
            
            if (numOfFenElements >= 3)
                CastlingAvailability = fenParts[2];

            if (numOfFenElements >= 4)
                EnPassantTargetSquare = fenParts[3];
            
            if (numOfFenElements >= 5)
                HalfMovesClock = fenParts[4];
            
            if (numOfFenElements >= 6)
                FullMoves = fenParts[5];
        }

        public Color GetNextPlayer() {
            if (ActiveColor == "b")
                return Color.Black;
            else
                return Color.White;
        }

        public IEnumerable<IPiece> GetBoardPieces() 
        {
            var row = 7;
            var col = 0;
            foreach(var c in PiecePlacement) {
                    if (c == '/') {
                        row--;
                        if (row < 0) throw new Exception($"Fen string contains too much rows! ( {PiecePlacement} )");
                        continue;
                    }

                    var position = new Position(col, row);
                    switch (c)
                    {
                        case '1': 
                        case '2': 
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            var number = Int32.Parse(c.ToString());
                            for (var ix = 0; ix < number; ix++)
                            {

                                var emptyCellPosition = new Position(col, row);
                                yield return new EmptyHouse(emptyCellPosition);
                                col++;
                                if (col > 7 ) 
                                    col = 0;
                            }
                            break;
                        case 'k':
                            yield return new King(position, Color.Black);
                            break;
                        case 'K':
                            yield return new King(position, Color.White);
                            break;
                        case 'q':
                            yield return new Queen(position, Color.Black);
                            break;
                        case 'Q':
                            yield return new Queen(position, Color.White);
                            break;
                        case 'p':
                            yield return new Pawn(position, Color.Black);
                            break;
                        case 'P':
                            yield return new Pawn(position, Color.White);
                            break;
                        case 'n':
                            yield return new Knight(position, Color.Black);
                            break;
                        case 'N':
                            yield return new Knight(position, Color.White);
                            break;
                        case 'b':
                            yield return new Bishop(position, Color.Black);
                            break;
                        case 'B':
                            yield return new Bishop(position, Color.White);
                            break;
                        case 'r':
                            yield return new Castle(position, Color.Black);
                            break;
                        case 'R':
                            yield return new Castle(position, Color.White);
                            break;
                        default:
                            throw new Exception($"{nameof(FenString)} unknown symbol {c}");
                    }
                    col++; 
                    if (col > 7) col = 0;

            }
        }
    }

    /// <summary>
    /// Fen ( https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation ) is a "string" notation
    /// used to represent the state of a chess board.
    /// </summary>
    public static class FenParser
    {
        public static BoardState FromFenString(this BoardState board, string fenString)
        {
            var fen = new FenString(fenString);

            board.NextPlayer = fen.GetNextPlayer();
            foreach(var piece in fen.GetBoardPieces())
                board.PutPiece(piece);

            return board;
        }
    }
}
