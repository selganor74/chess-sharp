using System;
using chess.core.Game;

namespace chess.player.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new ClassicGamePlayer();
            foreach(var m in game.Play())
            {
                Console.WriteLine(game.Board.ToString());
                // Console.WriteLine("Press a key for next move");
                // var k = Console.ReadKey();
            }
            if (game.Winner == Winner.Draw)
                Console.WriteLine("It's a draw!");
            else
                Console.Write($"The winner is {game.Winner.ToString()}");
        }
    }
}
