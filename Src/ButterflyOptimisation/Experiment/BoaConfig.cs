using Evo.Simulation.SimulationExperiment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ButterflyOptimisation.Experiment
{
    public class BoaConfig : ExperimentConfig
    {
        public BoaParameters BoaParameters { get; set; }
        public BoaExperimentParameters BoaExperimentParameters { get; set; }
    }
}
