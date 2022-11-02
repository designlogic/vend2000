using System;
using System.Collections.Generic;
using Vend2000.Interfaces;
using Vend2000.World.Coins;

namespace Vend2000.Modules
{
    public class CoinStorage : ICoinStorage
    {
        public int CoinCount => throw new NotImplementedException();

        public int Capacity => throw new NotImplementedException();

        public void Add(ICoin coin)
        {
            throw new NotImplementedException();
        }

        public List<ICoin> EmptyCoins()
        {
            throw new NotImplementedException();
        }
    }
}
