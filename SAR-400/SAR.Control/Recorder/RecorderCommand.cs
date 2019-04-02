using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SAR.Control.Costume;

namespace SAR.Control.Recorder
{
    public class RecorderCommand
    {
        public TimeSpan Duration { get; set; }
        public List<CostumeJoint> Joints { get; set; }
    }
}
