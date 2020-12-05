using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    public class Pawn : BasePiece
    {
        public override Kind Kind { get ; set ; } = Kind.Pawn;
        public bool IsFirstMove { get; private set; } = true;
        public bool CanBeTookEnPassant { get; private set; } = false;
        private BoardState _boardState;
        public override BoardState Board
        {
            get { return _boardState; }
            set
            {
                if (value == null)
                    _boardState?.UnregisterPlayerMoveHandler(OpponentsColor, OnOpponentsMove);

                if (value != null)
                    value.RegisterPlayerMoveHandler(OpponentsColor, OnOpponentsMove);

                _boardState = value;
            }
        }

        public Pawn(Position position, Color color) : base(position, color)
        {
        }

        public override List<Move> ValidMoves()
        {
            var direction = Color == Color.White ? 1 : -1;
            var toReturn = new List<Move>();
            var maxSteps = IsFirstMove ? 2 : 1;
            var currentStep = 1;
            Position nextPos;

            if (Board == null)
                throw new Exception("Can't move a piece not on a board");

            // straight moves
            do
            {
                nextPos = Position.MoveBy(direction * currentStep, 0);
                if (nextPos != null && Board.IsPositionFree(nextPos))
                    toReturn.Add(new Move { Piece = this, From = this.Position, To = nextPos });
                currentStep++;
            } while (currentStep <= maxSteps && nextPos != null && Board.IsPositionFree(nextPos));

            // takes
            foreach (int offsetX in new int[] { -1 /* Left take */, 1 /* right take */ })
            {
                var destPosition = Position.MoveBy(direction, offsetX);
                if (destPosition == null)
                    continue;
                var pieceToTake = Board.GetPieceAtPosition(destPosition);
                if (destPosition != null
                    && pieceToTake.IsOpponentOf(this))
                {
                    toReturn.Add(new Move { Piece = this, From = this.Position, To = destPosition, TookPiece = pieceToTake });
                    continue;
                }

                var enPassantTakePosition = Position.MoveBy(0, offsetX);
                var pawnToTake = Board.GetPieceAtPosition(enPassantTakePosition) as Pawn;
                if (pawnToTake != null && pawnToTake.CanBeTookEnPassant)
                    toReturn.Add(new Move { Piece = this, From = this.Position, To = destPosition, TookPiece = pawnToTake });
            }

            return toReturn;
        }

        public override void Move(Move move)
        {
            var deltaY = Math.Abs( move.From.Delta(move.To).Item2 );
            
            if (IsFirstMove && deltaY == 2) 
                CanBeTookEnPassant = true;  // will be reset by OnOpponentsMove handler

            IsFirstMove = false;
            base.Move(move);
        }

        private void OnOpponentsMove(Move move)
        {
            CanBeTookEnPassant = false;
        }        
    }
}