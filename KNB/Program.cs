using System;
using System.Collections.Generic;
using System.Linq;

namespace Papper
{
    public class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 3)
            {
                Console.WriteLine("Need at least three parameters");
                return;
            }

            if (args.Length % 2 != 1)
            {
                Console.WriteLine("The number of parameters must be odd");
                return;
            }

            if (args.GroupBy(x => x).Any(x => x.Count() > 1))
            {
                Console.WriteLine("Parameters must be unique");
                return;
            }

            var game = new Game(args.ToList());
            game.Run();
        }
    }
}

