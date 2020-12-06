using System;

namespace chess.core.Game
{
    [Serializable]
    public enum Kind
    {
        King = 'K',
        Queen = 'Q',
        Castle = 'R',
        Bishop = 'B',
        Knight = 'N',
        Pawn = 'P',
        Empty = ' '
    }
}
