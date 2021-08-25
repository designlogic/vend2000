using Vend2000.World.Coins;

namespace Vend2000.Interfaces
{
    public interface ICoinIdentifier
    {
        CoinType IdentifyCoinType(ICoin coin);
    }
}
