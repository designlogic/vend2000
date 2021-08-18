using System;
using System.Collections.Generic;
using System.Linq;
using Vend2000.World.Coins;
using Vend2000.World.Items;

namespace Vend2000
{
    public class Vend2000GumDispenser
    {
        private readonly ICoinValidator? coinValidator;
        private readonly IGumDispenser? gumDispenser;
        private readonly ICoinStorage? coinStorage;

        private string message = " ";

        private readonly string logo = @"
   _    __               _____   ____  ____  ____ 
  | |  / /__  ____  ____/ /__ \ / __ \/ __ \/ __ \
  | | / / _ \/ __ \/ __  /__/ // / / / / / / / / /
  | |/ /  __/ / / / /_/ // __// /_/ / /_/ / /_/ / 
  |___/\___/_/ /_/\__,_//____/\____/\____/\____/  
          ";

        private const string EscapeKeyCode = "\u001b";

        public Vend2000GumDispenser(ICoinValidator? coinValidator, IGumDispenser? gumDispenser, ICoinStorage? coinStorage)
        {
            this.coinValidator = coinValidator;
            this.gumDispenser = gumDispenser;
            this.coinStorage = coinStorage;
        }   

        public void Run()
        {
            while (true)
            {
                ClearScreen();
                Log(logo);

                LoadModules();

                var moduleIsMissing = (coinValidator == null || gumDispenser == null || coinStorage == null);
                if (moduleIsMissing)
                {
                    LineFeed();
                    Separator();
                    Log("Please install missing modules.");
                    Separator();
                    break;
                }

                ClearScreen();
                Log(logo);

                Log("=== Welcome to the Vend2000 Gum Dispenser ===");

                LineFeed();
                Separator();
                Log("Please insert one BRONZE coin:");
                Separator();

                Log("1. GOLD   coin");
                Log("2. SILVER coin");
                Log("3. BRONZE coin");

                DisplayMessage();

                var input = ReadKey();
                
                if (input == EscapeKeyCode)
                {
                    break;
                }

                if (input.ToLower() is "m")
                {
                    EnterMaintenanceMode();
                    ClearScreen();
                    continue;
                }

                var coin = GenerateCoinFromInput(input);
                if (coin is null)
                {
                    LineFeed();
                    BufferMessage("Invalid selection");
                    continue;
                }

                var coinType = coinValidator?.DetermineCoinType(coin);
                var coinIsInvalid = coinType != CoinType.Bronze;
                if (coinIsInvalid)
                {
                    LineFeed();
                    BufferMessage("Invalid coin");
                    ReturnCoin();
                    continue;
                }

                var gumPacket = gumDispenser?.Dispense();
                if (gumPacket is null)
                {
                    LineFeed();
                    BufferMessage("We apologize, we are currently out of Gum :(");
                    ReturnCoin();
                    continue;
                }

                coinStorage?.Add(coin);
                DispenseGum(gumPacket);
            }
        }

        private void BufferMessage(string message)
        {
            this.message += $"{Environment.NewLine}  {message}";
        }

        private void DisplayMessage()
        {
            LineFeed();
            Log(message);
            message = " ";
        }

        private void DispenseGum(GumPacket gumPacket)
        {
            LineFeed();
            BufferMessage("Clunk...");
            BufferMessage("Gum packet dispensed");
            BufferMessage("Enjoy!");
        }

        private void ReturnCoin()
        {
            LineFeed();
            BufferMessage("Coin returned");
        }

        private void EnterMaintenanceMode()
        {
            ClearScreen();
            Heading("Maintenance Mode");

            while (true)
            {
                Log("Enter Password or E to Exit:");
                var password = ReadPassword('*');

                if (password.ToLower() == "e")
                {
                    return;
                }

                if (password?.Trim() == "123")
                {
                    break;
                }

                Log("Password incorrect");
                LineFeed();
            }

            while (true)
            {
                ClearScreen();
                Heading("Maintenance Mode");
                LineFeed();

                Log($"Gum packets {gumDispenser?.Quantity} of {gumDispenser?.Capacity}");
                Log($"Coin count {coinStorage?.CoinCount}");
                Separator();
                LineFeed();

                Log("Please choose an operation:");
                Log("1. Load Gum packet");
                Log("2. Dispense Gum packet");
                Log("3. Empty coin storage");
                Log("4. Exit Maintenance Mode");

                var input = ReadNumberedInput();

                if (input is 4)
                {
                    break;
                }

                switch (input)
                {
                    case 1:
                        gumDispenser?.Add(new GumPacket());
                        break;
                    case 2:
                        gumDispenser?.Dispense();
                        break;
                    case 3:
                        coinStorage?.Empty();
                        break;
                }
            }
        }

        public static string ReadPassword(char mask)
        {
            const int ENTER = 13, BACKSP = 8, CTRLBACKSP = 127;
            int[] FILTERED = {0, 27, 9, 10 /*, 32 space, if you care */}; // const

            var pass = new Stack<char>();
            var chr = (char) 0;

            while ((chr = Console.ReadKey(true).KeyChar) != ENTER)
            {
                if (chr == BACKSP)
                {
                    if (pass.Count > 0)
                    {
                        Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (chr == CTRLBACKSP)
                {
                    while (pass.Count > 0)
                    {
                        Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (FILTERED.Count(x => chr == x) > 0)
                {
                }
                else
                {
                    pass.Push(chr);
                    Console.Write(mask);
                }
            }

            Console.WriteLine();

            return new string(pass.Reverse().ToArray());
        }

        private ICoin? GenerateCoinFromInput(string input)
        {
            var key = ReadNumberedInput(input);

            switch (key)
            {
                case 1: return new GoldCoin();
                case 2: return new SilverCoin();
                case 3: return new BronzeCoin();
            }

            return null;
        }

        private void LoadModules()
        {
            Log($"Verifying Module Installation... ");
            LineFeed();

            var coinValidatorMessage = coinValidator is null ? "*** Not installed *** (Program.cs Line 11)" : "Installed";
            var gumDispenserMessage = gumDispenser is null ? "*** Not installed *** (Program.cs Line 12)" : "Installed";
            var coinStorageMessage = coinStorage is null ? "*** Not installed *** (Program.cs Line 13)" : "Installed";
            
            Log($"Coin Validator module : {coinValidatorMessage}");
            Log($"Gum Dispenser module  : {gumDispenserMessage}");
            Log($"Coin Storage module   : {coinStorageMessage}");
            
        }

        #region Helpers

        private void Heading(string heading)
        {
            Separator('=');
            Log(heading?.ToUpper() ?? "");
            Separator('=');
        }

        private void Log(string message = "", bool important = false)
        {
            if (important)
            {
                message = $"*** {message} ***";
            }

            Console.WriteLine($"  {message}");
        }

        private void LineFeed()
        {
            Console.WriteLine("");
        }

        private void Separator(char separator = '-')
        {
            Console.WriteLine("".PadLeft(50,separator));
        }

        private void ClearScreen()
        {
            Console.Clear();
        }


        private int ReadNumberedInput()
        {
            var input = Console.ReadKey(true);
            return ReadNumberedInput(input.KeyChar.ToString());
        }

        private int ReadNumberedInput(string input)
        {
            int.TryParse(input, out var key);

            return key < 11 ? key : key - 48;
        }


        private string ReadKey()
        {
            var input = Console.ReadKey(true);
            return input.KeyChar.ToString();
        }

        #endregion
    }
}