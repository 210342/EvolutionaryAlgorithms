using Evo.Simulation.SimulationExperiment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ButterflyOptimisation.Experiment
{
    public class BoaParameters : InputParameters
    {
        public double SensorModality { get; set; }
        public double PowerExponent { get; set; }
        public double SwitchProbability { get; set; }
        public int SubPopulationCount { get; set; }
        public int SwarmPermutePeriod { get; set; } = 1;
    }
}
