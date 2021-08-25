using System;
using Vend2000.Interfaces;

namespace Vend2000
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            ICoinIdentifier coinIdentifier = null;
            IGumDispenser gumDispenser = null;
            ICoinStorage coinStorage = null;

            var vend2000 = new Vend2000GumDispenser(coinIdentifier, gumDispenser, coinStorage);
            vend2000.Run();

            Console.CursorVisible = true;
        }
    }
}
