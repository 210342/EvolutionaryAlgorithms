using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation
{
    public class InputParameters
    {
        public Range[] UniverseSize { get; set; }
        public int MaxEpoch { get; set; }
        public int PopulationSize { get; set; }
        public string StopCondition { get; set; }
    }
}
