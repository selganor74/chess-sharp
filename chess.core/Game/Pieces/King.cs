using System.Linq;
using System;
using System.Collections.Generic;
using chess.core.Game.Moves;

namespace chess.core.Game
{
    [Serializable]
    public class King : BaseBishopCastleQueenKing
    {
        public bool IsFirstMove {get;set;} = true;
        public override Kind Kind {get;set;} = Kind.King;
        protected override int MaxSteps { get {return 1; } }

        protected override List<Tuple<int, int>> Directions {get;} = new List<Tuple<int, int>>() {
                new Tuple<int,int>( 1,  1),
                new Tuple<int,int>( 0,  1),
                new Tuple<int,int>(-1,  1),
                new Tuple<int,int>(-1,  0),
                new Tuple<int,int>(-1, -1),
                new Tuple<int,int>( 0, -1),
                new Tuple<int,int>( 1, -1),
                new Tuple<int,int>( 1,  0),
            };

        public King(Position position, Color color) : base(position, color)
        {
        }

        public override void Move(Move move)
        {
            IsFirstMove = false;
            base.Move(move);
        }

        public override List<Move> ValidMoves()
        {
            var validMoves = base.ValidMoves();

            // Castling is a move the king can do only on its first move and must also be the rook's first move.
            if (IsFirstMove) {
                var castles = Board.GetCastlesForPlayer(Color).Where(c => c.IsFirstMove);
                var kingPosition = Position;
                foreach(var c in castles) {
                    var d = c.Position.Column - kingPosition.Column;
                    var direction = Math.Sign(d); // direction for x checks
                    var canDoCastling = true;
                    for(var step = 1; step < Math.Abs(d); step ++)
                    {
                        var newPos = kingPosition.MoveBy(0, direction * step);
                        if (!Board.IsPositionFree(newPos)) {
                            canDoCastling = false;
                            break;
                        }
                    }
                    if (canDoCastling) {
                        var newKingPosition = kingPosition.MoveBy(0, 2 * direction);
                        var newCastlePosition = newKingPosition.MoveBy(0, -1 * direction);
                        validMoves.Add(new CastlingMove(this, newKingPosition, c, newCastlePosition));
                    }
                }
            }            
            return validMoves;
        }

        public override IPiece Clone()
        {
            var toReturn = new King(new Position(Position.AsIndex), Color);
            toReturn.IsFirstMove = IsFirstMove;
            return toReturn;
        }
    }
}
