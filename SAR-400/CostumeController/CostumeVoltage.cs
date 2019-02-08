using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostumeController
{
    public class CostumeVoltage
    {
        public float Value { get; private set; }
        public PackedData Raw { get { return _raw; } }
        public float valueScaler = 1;

        private PackedData _raw = new PackedData();

        public void Update()
        {
            Value = _raw.Value * valueScaler;
        }
    }
}
