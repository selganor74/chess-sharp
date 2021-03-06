using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using chess.core.Game.Moves;

namespace chess.core.Game
{
    [Serializable]
    public class Pawn : BasePiece
    {
        public override Kind Kind { get; set; } = Kind.Pawn;
        public bool IsFirstMove { get; private set; } = true;
        public bool CanBeTookEnPassant { get; private set; } = false;
        
        [IgnoreDataMemberAttribute]
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
                    toReturn.Add(new SimpleMove(this, nextPos));

                currentStep++;
            } while (currentStep <= maxSteps && nextPos != null && Board.IsPositionFree(nextPos));

            // takes
            foreach (int offsetX in new int[] { -1 /* Left take */, 1 /* right take */ })
            {
                var destPosition = Position.MoveBy(direction, offsetX);
                if (destPosition == null)
                    continue;

                var pieceToTake = Board.GetOpponentAtPosition(destPosition, this);
                if (pieceToTake != null)
                {
                    toReturn.Add(new TakeMove(this, destPosition, pieceToTake));
                    continue;
                }

                var enPassantTakePosition = Position.MoveBy(0, offsetX);
                var pawnToTake = Board.GetPieceAtPosition(enPassantTakePosition) as Pawn;
                if (pawnToTake != null && pawnToTake.IsOpponentOf(this) && pawnToTake.CanBeTookEnPassant)
                    toReturn.Add(new TakeEnPassantMove(this, destPosition, pawnToTake) );
            }

            return toReturn;
        }

        public override void Move(Move move)
        {
            var deltaY = Math.Abs(move.From.Delta(move.To).Item2);

            if (IsFirstMove && deltaY == 2)
                CanBeTookEnPassant = true;  // will be reset by OnOpponentsMove handler

            IsFirstMove = false;
            base.Move(move);
        }

        private void OnOpponentsMove(Move move)
        {
            CanBeTookEnPassant = false;
        }

        public override IPiece Clone()
        {
            var toReturn = new Pawn(new Position(Position.AsIndex), Color);
            toReturn.IsFirstMove = this.IsFirstMove;
            toReturn.CanBeTookEnPassant = this.CanBeTookEnPassant;
            return toReturn;
        }
    }
}