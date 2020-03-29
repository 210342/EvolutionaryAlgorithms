using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation
{
    public class InputParameters
    {
        public uint MaxEpoch { get; set; }
        public uint PopulationSize { get; set; }
        public string StopCondition { get; set; }
        public double MinAccuracy { get; set; }
    }
}
