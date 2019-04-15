using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAR.Control.Costume
{
    public class CostumeJoint
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public PackedData Raw { get; } = new PackedData();

        public float rawValueOffset;
        public float valueScaler = 1;

        public void Update()
        {
            Value = (Raw.Value + rawValueOffset) * valueScaler;
        }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}
