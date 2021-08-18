using Vend2000.World;
using Vend2000.World.Coins;
using Vend2000.World.Items;

namespace Vend2000
{
    public class Vend2000GumDispenser : Vend2000Base
    {
        private readonly ICoinValidator? coinValidator;
        private readonly IGumDispenser? gumDispenser;
        private readonly ICoinStorage? coinStorage;

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
                    AppendMessage("Invalid selection");
                    continue;
                }

                var coinType = coinValidator?.DetermineCoinType(coin);
                var coinIsInvalid = coinType != CoinType.Bronze;
                if (coinIsInvalid)
                {
                    LineFeed();
                    AppendMessage("Invalid coin");
                    ReturnCoin();
                    continue;
                }

                var gumPacket = gumDispenser?.Dispense();
                if (gumPacket is null)
                {
                    LineFeed();
                    AppendMessage("We apologize, we are currently out of Gum :(");
                    ReturnCoin();
                    continue;
                }

                coinStorage?.Add(coin);
                DispenseGum(gumPacket);
            }
        }

        private void DispenseGum(GumPacket gumPacket)
        {
            LineFeed();
            AppendMessage("Clunk...");
            AppendMessage("Gum packet dispensed");
            AppendMessage("Enjoy!");
        }

        private void ReturnCoin()
        {
            LineFeed();
            AppendMessage("Coin returned");
        }

        private void EnterMaintenanceMode()
        {
            ClearScreen();
            Heading("Maintenance Mode");

            while (true)
            {
                Log("Enter Password or E to Exit:");
                var password = ReadPassword();

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
                    case 4:
                        break;
                }
            }
        }

        private ICoin? GenerateCoinFromInput(string input)
        {
            var key = ReadNumberedInput(input);

            return key switch
            {
                1 => new GoldCoin(),
                2 => new SilverCoin(),
                3 => new BronzeCoin(),
                _ => null
            };
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
    }
}