
using Vend2000.World.Items;

namespace Vend2000.Interfaces
{
    public interface IGumDispenser
    {
        int Capacity { get; }
        int Quantity { get; }
        void Add(GumPacket gumPacket);
        GumPacket? Dispense();
    }
}
