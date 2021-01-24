using System;
using System.Collections.Generic;
using System.Linq;

namespace chess.core.Game
{
    public static class ClassicGamePiecePoints
    {
        public static readonly int QueenPoints = 9;
        public static readonly int CastlePoints = 5;
        public static readonly int BishopPoints = 3;
        public static readonly int KnightPoints = 3;
        public static readonly int PawnPoints = 1;
        public static readonly int KingPoints = 0;

        public static int Points(this IPiece piece)
        {
            var @switch = new Dictionary<Type, int>()
            {
                {typeof(Queen), QueenPoints},
                {typeof(Castle), CastlePoints},
                {typeof(Bishop), BishopPoints},
                {typeof(Knight), KnightPoints},
                {typeof(Pawn), PawnPoints},
                {typeof(King), KingPoints},
            };

            return @switch[piece.GetType()];
        }

        public static int Points(this King piece)
        {
            return KingPoints;
        }

        public static int Points(this Queen piece)
        {
            return QueenPoints;
        }

        public static int Points(this Castle piece)
        {
            return CastlePoints;
        }

        public static int Points(this Bishop piece)
        {
            return BishopPoints;
        }

        public static int Points(this Knight piece)
        {
            return KnightPoints;
        }

        public static int Points(this Pawn piece)
        {
            return PawnPoints;
        }

        public static int InitialPoints(this BoardState board)
        {
            return QueenPoints + 2 * BishopPoints + 2 * CastlePoints + 2 * KnightPoints + 8 * PawnPoints;
        }
    }


    [Serializable]
    public static class ClassicGame
    {
        public static void InitClassicGame(this BoardState board)
        {
            // Pawns
            for (int p = 0; p <= 7; p++)
            {
                var col = ((char)('a' + p)).ToString();
                var whitePawnPosition = new Position(col + "2");
                var blackPawnPosition = new Position(col + "7");
                var w = new Pawn(whitePawnPosition, Color.White);
                var b = new Pawn(blackPawnPosition, Color.Black);
                board.PutPiece(w);
                board.PutPiece(b);
            }

            // Castles
            var wc1 = new Castle(new Position("a1"), Color.White);
            var wc2 = new Castle(new Position("h1"), Color.White);
            var bc1 = new Castle(new Position("a8"), Color.Black);
            var bc2 = new Castle(new Position("h8"), Color.Black);
            board.PutPiece(wc1);
            board.PutPiece(wc2);
            board.PutPiece(bc1);
            board.PutPiece(bc2);

            // Knights
            var wk1 = new Knight(new Position("b1"), Color.White);
            var wk2 = new Knight(new Position("g1"), Color.White);
            var bk1 = new Knight(new Position("b8"), Color.Black);
            var bk2 = new Knight(new Position("g8"), Color.Black);
            board.PutPiece(wk1);
            board.PutPiece(wk2);
            board.PutPiece(bk1);
            board.PutPiece(bk2);

            // Bishops
            var wb1 = new Bishop(new Position("c1"), Color.White);
            var wb2 = new Bishop(new Position("f1"), Color.White);
            var bb1 = new Bishop(new Position("c8"), Color.Black);
            var bb2 = new Bishop(new Position("f8"), Color.Black);
            board.PutPiece(wb1);
            board.PutPiece(wb2);
            board.PutPiece(bb1);
            board.PutPiece(bb2);

            // Royal Family
            var wq = new Queen(new Position("d1"), Color.White);
            var wk = new King(new Position("e1"), Color.White);
            var bq = new Queen(new Position("d8"), Color.Black);
            var bk = new King(new Position("e8"), Color.Black);
            board.PutPiece(wq);
            board.PutPiece(wk);
            board.PutPiece(bq);
            board.PutPiece(bk);
        }

        public static int EvaluateSituation(this BoardState board, Color forPlayer)
        {
            int toReturn = 0;

            var forOpponent = forPlayer == Color.White ? Color.Black : Color.White;
            var playersPieces = board.GetPieces(forPlayer);
            var playerPoints = playersPieces.Sum(p => p.Points());

            var opponentsPieces = board.GetPieces(forOpponent);
            var opponentsPoints = board.InitialPoints() - opponentsPieces.Sum(p => p.Points());

            toReturn = playerPoints + opponentsPoints;

            return toReturn;
        }

        public static int EvaluateMove(this BoardState board, Move move) 
        {
            var simulated = board.SimulateMove(move);
            var evaluation = simulated.EvaluateSituation(board.NextPlayer);
            return evaluation;
        }
    }
}
