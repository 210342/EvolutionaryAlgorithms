using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ParticleSwarm
{
    internal class SubSwarm : Swarm
    {
        public SubSwarm(IUniverse<IPopulation<Particle>, Particle> universe, Particle[] organisms) 
            : base(universe, organisms) { }

        public override string ToString()
        {
            return Result?.ToString();
        }
    }
}
