using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation
{
    public class InputParameters
    {
        public uint PopulationSize { get; set; }
        public uint MaxEpoch { get; set; }
        public string StopCondition { get; set; }
        public double MinAccuracy { get; set; }

        public override string ToString()
        {
            return $"{PopulationSize};{MaxEpoch};{StopCondition};{MinAccuracy}";
        }
    }
}
