using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vend2000
{
    public class CoinValidator : ICoinValidator
    {


        //public IResult<bool> Validate(ICoin coin, CoinType coinType)
        //{
        //    if (coin.CoinType != coinType)
        //    {
        //        return Result.Fail(false, $"Invalid coin: {coin.Name}");
        //    }

        //    return Result.Success(true);
        //}
        
        public CoinType DetermineCoinType(ICoin coin)
        {
            if (coin.Diameter == 30 && coin.Weight == 11)
            {
                return CoinType.Gold;
            }

            if (coin.Diameter == 24 && coin.Weight == 5)
            {
                return CoinType.Silver;
            }

            if (coin.Diameter == 17 && coin.Weight == 2)
            {
                return CoinType.Bronze;
            }

            return CoinType.Unknown;
        }
    }

    public class GumDispenser10Unit : IGumDispenser
    {
        private readonly Queue<GumPacket> queue = new Queue<GumPacket>(10);

        public GumDispenser10Unit()
        {
            queue.Enqueue(new GumPacket());
            queue.Enqueue(new GumPacket());
            queue.Enqueue(new GumPacket());
        }

        public int Capacity => 10;
        public int Quantity => queue.Count;

        public void Add(GumPacket gumPacket)
        {
            if (Quantity >= Capacity)
            {
                return;
            }

            queue.Enqueue(gumPacket);
        }

        GumPacket IGumDispenser.Dispense()
        {
            if (Quantity <= 0)
            {
                return null;
            }

            return queue.Dequeue();
        }
    }

    public class CoinStorage : ICoinStorage
    {
        public int CoinCount => coins.Count;
        private readonly List<ICoin> coins = new List<ICoin>();

        public List<ICoin> Empty()
        {
            var coinsCopy = coins.Select(coin => coin).ToList();
            coins.Clear();
            return coinsCopy;
        }

        public void Add(ICoin coin)
        {
            coins.Add(coin);
        }
    }
}
