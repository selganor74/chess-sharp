using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Transactions;

namespace chess.core.Game
{
    [Serializable]
    public enum Color
    {
        Black,
        White,
        None /* for empty houses */
    }
}
