using Evo.ParticleSwarm.Experiment;
using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class MultiSwarmUniverse : SwarmUniverse
    {
        public override IPopulation<Particle> Population { get; }

        public MultiSwarmUniverse(
            SwarmParameters swarmParameters,
            Simulation.Range[] universeSize,
            (Func<double[], double>, Func<double, double, bool>) functions)
            : base(swarmParameters, universeSize, functions)
        {
            Population = new MultiSwarm(this, swarmParameters);
            Swarm = null;
        }
    }
}
