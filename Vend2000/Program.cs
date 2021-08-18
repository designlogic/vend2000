using System;

namespace Vend2000
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            ICoinValidator coinValidator = null;
            IGumDispenser gumDispenser = null;
            ICoinStorage coinStorage = null;

            var vend2000 = new Vend2000GumDispenser(coinValidator, gumDispenser, coinStorage);
            vend2000.Run();

            Console.CursorVisible = true;
        }
    }
}
