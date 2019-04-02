using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAR.Control.Costume
{
    public class CostumeVoltage
    {
        public float Value { get; private set; }
        public PackedData Raw { get; } = new PackedData();
        public float valueScaler = 1;

        public void Update()
        {
            Value = Raw.Value * valueScaler;
        }
    }
}
