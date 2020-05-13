using Evo.Simulation.SimulationExperiment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ButterflyOptimisation.Experiment
{
    public class BoaExperimentParameters : ExperimentParameters
    {
        public BoaParameters Min { get; set; }
        public BoaParameters Rate { get; set; }
        public BoaParameters Max { get; set; }
    }
}
