using System;
using System.Collections.Generic;
using System.Linq;

namespace Vend2000.World
{
    public class Vend2000Base
    {
        protected string message = " ";

        protected readonly string logo = @"
   _    __               _____   ____  ____  ____ 
  | |  / /__  ____  ____/ /__ \ / __ \/ __ \/ __ \
  | | / / _ \/ __ \/ __  /__/ // / / / / / / / / /
  | |/ /  __/ / / / /_/ // __// /_/ / /_/ / /_/ / 
  |___/\___/_/ /_/\__,_//____/\____/\____/\____/  
          ";

        protected const string EscapeKeyCode = "\u001b";

        #region Helpers

        protected void Heading(string heading)
        {
            Separator('=');
            Log(heading?.ToUpper() ?? "");
            Separator('=');
        }

        protected void LogError(string logMessage) => Log(logMessage, false, ConsoleColor.Red);
        protected void LogSuccess(string logMessage) => Log(logMessage, false, ConsoleColor.Green);

        protected void Log(string logMessage = "", bool important = false, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            if (important)
            {
                logMessage = $"*** {logMessage} ***";
            }

            Console.ForegroundColor = foregroundColor;
            Console.WriteLine($"  {logMessage}");
            Console.ResetColor();
        }

        protected void LineFeed()
        {
            Console.WriteLine("");
        }

        protected void Separator(char separator = '-')
        {
            Console.WriteLine("".PadLeft(50, separator));
        }

        protected void ClearScreen()
        {
            Console.Clear();
        }

        protected int ReadNumberedInput()
        {
            var input = Console.ReadKey(true);
            return ReadNumberedInput(input.KeyChar.ToString());
        }

        protected int ReadNumberedInput(string input)
        {
            int.TryParse(input, out var key);

            return key < 11 ? key : key - 48;
        }


        protected string ReadKey()
        {
            var input = Console.ReadKey(true);
            return input.KeyChar.ToString();
        }

        protected void AppendMessage(string message)
        {
            this.message += $"{Environment.NewLine}  {message}";
        }

        protected void DisplayMessage()
        {
            LineFeed();
            Log(message);
            message = " ";
        }

        protected string ReadPassword()
        {
            const int enter = 13;
            const int backspace = 8;
            const int controlBackspace = 127;

            int[] filtered = { 0, 27, 9, 10 };

            var password = new Stack<char>();
            var character = (char)0;

            while ((character = Console.ReadKey(true).KeyChar) != enter)
            {
                var characterIsInvalid = filtered.Count(x => character == x) > 0;
                if (characterIsInvalid)
                {
                    continue;
                }


                if (character == backspace && password.Count > 0)
                {
                    Console.Write("\b \b");
                    password.Pop();
                    continue;
                }

                if (character == controlBackspace)
                {
                    while (password.Count > 0)
                    {
                        Console.Write("\b \b");
                        password.Pop();
                    }

                    continue;
                }

                password.Push(character);
                Console.Write("*");
            }

            Console.WriteLine();

            return new string(password.Reverse().ToArray());
        }

        #endregion
    }
}