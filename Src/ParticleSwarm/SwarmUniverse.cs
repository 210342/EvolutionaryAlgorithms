using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class SwarmUniverse : Universe<Particle, IPopulation<Particle>>, IUniverse<IPopulation<Particle>, Particle>
    {
        public SwarmParameters Parameters { get; }
        public override IPopulation<Particle> Population => Swarm;
        public Swarm Swarm { get; }

        public SwarmUniverse(
            SwarmParameters swarmParameters,
            (Func<double[], double>, Func<double, double, bool>) functions)
            : base(swarmParameters.UniverseSize, swarmParameters.MaxEpoch, functions)
        {
            Parameters = swarmParameters;
            Swarm = new Swarm(this, swarmParameters.PopulationSize);
        }
    }
}
