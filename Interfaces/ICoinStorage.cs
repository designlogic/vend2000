using System.Collections.Generic;
using Vend2000.World.Coins;

namespace Vend2000.Interfaces
{
    public interface ICoinStorage
    {
        List<ICoin> EmptyCoins();
        void Add(ICoin coin);
        int CoinCount { get; }
        int Capacity { get; }
    }
}