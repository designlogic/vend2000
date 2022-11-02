using System;
using Vend2000.Interfaces;
using Vend2000.World.Items;

namespace Vend2000.Modules
{
    public class GumDispenser : IGumDispenser
    {
        public int Capacity => throw new NotImplementedException();

        public int Quantity => throw new NotImplementedException();

        public void Add(GumPacket gumPacket)
        {
            throw new NotImplementedException();
        }

        public GumPacket? Dispense()
        {
            throw new NotImplementedException();
        }
    }
}
