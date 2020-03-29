using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation
{
    public class InputParameters
    {
        public Range[] UniverseSize { get; set; }
        public uint MaxEpoch { get; set; }
        public uint PopulationSize { get; set; }
        public string StopCondition { get; set; }
    }
}
