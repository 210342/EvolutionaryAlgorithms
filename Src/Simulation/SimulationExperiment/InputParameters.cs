using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation.SimulationExperiment
{
    public class InputParameters
    {
        public uint PopulationSize { get; set; }
        public uint MaxEpoch { get; set; }
        public string StopCondition { get; set; }
        public double MinAccuracy { get; set; }

        public InputParameters() { }

        public InputParameters(InputParameters other)
        {
            PopulationSize = other.PopulationSize;
            MaxEpoch = other.MaxEpoch;
            StopCondition = other.StopCondition;
            MinAccuracy = other.MinAccuracy;
        }

        public override string ToString()
        {
            return $"{PopulationSize};{MaxEpoch};{StopCondition};{MinAccuracy}";
        }
    }
}
