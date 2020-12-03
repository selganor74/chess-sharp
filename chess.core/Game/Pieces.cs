using System;
using System.Collections.Generic;

namespace chess.core.Game
{
    public class EmptyHouse : IPiece
    {
        public Position Position { get; set; }
        public Kind Kind { get; set; } = Kind.Empty;
        public Color Color { get; set; } = Color.None;
        public BoardState Board { get; set; }

        public Color OpponentsColor => Color.None;

        public EmptyHouse(Position position)
        {
            Position = position;
        }
        public List<Move> ValidMoves()
        {
            return new List<Move>();
        }
        public void Move(string to)
        {
            throw new Exception("Can't move an empty house!");
        }

        public void Move(Position to)
        {
            throw new Exception("Can't move an empty house!");
        }

        public bool IsOpponentOf(IPiece other)
        {
            return false;
        }

        public bool IsEmptyHouse()
        {
            return true;
        }

        public void Move(string to, IPiece tookPiece = null)
        {
            throw new Exception("Can't move an empty house!");
        }

        public void Move(Position to, IPiece tookPiece = null)
        {
            throw new Exception("Can't move an empty house!");
        }

        public void Move(Move move)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class BasePiece : IPiece
    {
        public Position Position { get; set; }
        public abstract Kind Kind { get; set; }
        public Color Color { get; set; }
        public virtual BoardState Board { get; set; }

        public Color OpponentsColor => Color == Color.None ? Color.None : (Color == Color.White ? Color.Black : Color.White);

        public BasePiece(Position position, Color color)
        {
            Position = position;
            Color = color;
        }
        public abstract List<Move> ValidMoves();

        public virtual void Move(string to, IPiece tookPiece = null)
        {
            Move(new Position(to), tookPiece);
        }

        public virtual void Move(Position to, IPiece tookPiece = null)
        {
            var move = new Move() { Piece = this, From = this.Position, To = to, TookPiece = tookPiece };
            Move(move);
        }
        public virtual void Move(Move move)
        {
            Board.MakeMove(move);
        }

        public bool IsOpponentOf(IPiece other)
        {
            return !other.IsEmptyHouse() && other.Color != Color;
        }

        public bool IsEmptyHouse()
        {
            return false;
        }
    }

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
            var currentStep = 0;
            Position nextPos;

            if (Board == null)
                throw new Exception("Can't move a piece not on a board");

            // straight moves
            do
            {
                nextPos = Position.MoveBy(direction, 0);
                if (nextPos != null && Board.IsPositionFree(nextPos))
                    toReturn.Add(new Move { Piece = this, From = this.Position, To = nextPos });
                currentStep++;
            } while (currentStep < maxSteps && nextPos != null && Board.IsPositionFree(nextPos));

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
            if (IsFirstMove) // will be reset by the board ...
                CanBeTookEnPassant = true;

            IsFirstMove = false;
            base.Move(move);
        }

        private void OnOpponentsMove(Move move)
        {
            CanBeTookEnPassant = false;
        }        
    }
}