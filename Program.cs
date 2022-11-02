using System;

namespace Vend2000
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            var vend2000 = new Vend2000GumDispenser();
            vend2000.Run();

            Console.CursorVisible = true;
        }
    }
}
