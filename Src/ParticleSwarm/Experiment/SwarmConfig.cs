using Evo.ParticleSwarm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ParticleSwarm.Experiment
{
    public class SwarmConfig
    {
        public bool SingleSimulation { get; set; }
        public int FunctionIndex { get; set; }
        public SwarmParameters SwarmParameters { get; set; }
        public SwarmExperimentParameters ExperimentParameters { get; set; }
    }
}
