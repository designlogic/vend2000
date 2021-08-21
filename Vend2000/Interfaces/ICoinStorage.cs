using System.Collections.Generic;
using Vend2000.World.Coins;

namespace Vend2000.Interfaces
{
    public interface ICoinStorage
    {
        List<ICoin> Empty();
        void Add(ICoin coin);
        int CoinCount { get; }
        int Capacity { get; }
    }

    //This is abstract test
}