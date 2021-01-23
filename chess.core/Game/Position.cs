using System;
using System.IO.Enumeration;
using System.Xml.Serialization;

namespace chess.core.Game
{
    [Serializable]
    public class Position
    {
        public string AsString { get; }
        public int AsIndex { get; }
        public int Row {get;} // from 0 to 7
        public int Column {get;} // from 0 to 7

        public Position(string fromString)
        {
            fromString = fromString.ToLower();
            var col = fromString[0]; // a, b, c, ... 
            var row = fromString.Substring(1, 1); // 1, 2, 3, ...

            int x = col - 'a'; // starting from 0
            int y = int.Parse(row) - 1; // starting from 0

            Row = y;
            Column = x;
            AsString = fromString;
            AsIndex = (8 * y) + x;
        }

        public Position(int fromIndex)
        {
            int x = fromIndex % 8; // starting from 0
            int y = fromIndex / 8; // starting from 0

            var col = ((char)('a' + x)).ToString();
            var row = (y + 1).ToString();

            Row = y;
            Column = x;
            AsString = col + row;
            AsIndex = fromIndex;
        }

        public Position(int zeroBasedColumn, int zeroBasedRow) : this(fromIndex: zeroBasedRow*8 + zeroBasedColumn)
        {
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

        public Tuple<int, int> Delta(Position other) 
        {
            int currX = AsIndex % 8;
            int currY = AsIndex / 8;

            int otherX = other.AsIndex % 8;
            int otherY = other.AsIndex / 8;
            
            var toReturn = new Tuple<int, int>(otherX - currX, otherY - currY);
            
            return toReturn;
        } 
    }
}
