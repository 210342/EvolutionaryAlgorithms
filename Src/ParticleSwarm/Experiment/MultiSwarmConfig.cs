using Evo.ButterflyOptimisation.Experiment;
using Evo.Simulation.SimulationExperiment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ParticleSwarm.Experiment
{
    public class MultiSwarmConfig : ExperimentConfig
    {
        public BoaConfig BoaConfig { get; set; }
        public SwarmConfig SwarmConfig { get; set; }
    }
}
