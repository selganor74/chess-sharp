using System;

namespace chess.core.Game
{
    public static class ClassicGame
    {
        public static void InitClassicGame(this BoardState board) 
        {
            // Pawns
            for(int p = 0; p <= 7; p++) {
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
            var wk = new King( new Position("e1"), Color.White);
            var bq = new Queen(new Position("d8"), Color.Black);
            var bk = new King( new Position("e8"), Color.Black);
            board.PutPiece(wq);
            board.PutPiece(wk);
            board.PutPiece(bq);
            board.PutPiece(bk);
        }
    }
}
