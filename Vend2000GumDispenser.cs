using Vend2000.Interfaces;
using Vend2000.Modules;
using Vend2000.World;
using Vend2000.World.Coins;
using Vend2000.World.Items;

namespace Vend2000
{
    public class Vend2000GumDispenser : Vend2000Base
    {
        private readonly ICoinIdentifier coinIdentifier = new CoinIdentifier();
        private readonly IGumDispenser gumDispenser = new GumDispenser();
        private readonly ICoinStorage coinStorage = new CoinStorage();
        
        public void Run()
        {
            while (true)
            {
                ClearScreen();
                Log(logo);

                var moduleMalfunction = !IsSystemTestSuccessful();
                if (moduleMalfunction)
                {
                    LineFeed();
                    Separator();
                    Log("*** Module malfunction detected ***");
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

                var coinType = coinIdentifier?.IdentifyCoinType(coin);
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
                        coinStorage?.EmptyCoins();
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

        private bool IsSystemTestSuccessful()
        {
            Log($"Checking modules... ");
            LineFeed();

            var hasError = false;

            try
            {
                var _ = coinIdentifier.IdentifyCoinType(new GoldCoin());
                Log($"Coin Identifier module: Installed");
            }
            catch 
            {
                hasError = true;
                Log($"Coin Identifier module: *** Malfunction ***");
            }

            try
            {
                var capacity = gumDispenser.Capacity;
                var quantity = gumDispenser.Quantity;
                gumDispenser.Add(new GumPacket());
                var _ = gumDispenser.Dispense();

                Log($"Gum Dispenser module: Installed");
            }
            catch
            {
                hasError = true;
                Log($"Gum Dispenser module: *** Malfunction ***");
            }

            try
            {
                var capacity = coinStorage.Capacity;
                var count = coinStorage.CoinCount;
                coinStorage.Add(new GoldCoin());
                var _ = coinStorage.EmptyCoins();

                Log($"Coin Storage module: Installed");
            }
            catch
            {
                hasError = true;
                Log($"Coin Storage module: *** Malfunction ***");
            }

            return !hasError;
        }
    }
}