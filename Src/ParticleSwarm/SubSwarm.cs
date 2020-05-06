using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class SubSwarm : Swarm
    {
        public Particle EliteParticle { get; private set; }

        public SubSwarm(IUniverse<IPopulation<Particle>, Particle> universe, Particle[] organisms) 
            : base(universe, organisms) 
        {
            EliteParticle = Organisms.Aggregate((p1, p2) => Universe.FitnessFunction(p1.BestValue, p2.BestValue) ? p1 : p2);
        }

        public override void Evolve()
        {
            if (SwarmParameters.UseEliteParticles)
            {
                EliteParticle.SuspendEvolution = true;
            }
            base.Evolve();
            EliteParticle.SuspendEvolution = false;
            EliteParticle = Organisms.Aggregate((p1, p2) => Universe.FitnessFunction(p1.BestValue, p2.BestValue) ? p1 : p2);
        }

        public override string ToString()
        {
            return Result?.ToString();
        }
    }
}
