using Evo.Simulation.SimulationExperiment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ParticleSwarm.Experiment
{
    public class SwarmExperimentParameters : ExperimentParameters
    {
        public SwarmParameters Min { get; set; }
        public SwarmParameters Rate { get; set; }
        public SwarmParameters Max { get; set; }
    }
}
