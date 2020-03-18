using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class Swarm : Population<Particle>
    {
        private double[] bestPosition;

        public double BestValue { get; set; }
        public double[] BestPosition { get => bestPosition; set => value.CopyTo(bestPosition.AsSpan()); }
        public double ChangeParameter { get; }
        internal SwarmUniverse SwarmUniverse => Universe as SwarmUniverse;

        internal Swarm(IUniverse<IPopulation<Particle>, Particle> universe, int populationSize) :
            base(universe, Enumerable.Repeat(new Particle(universe), populationSize))
        {
            bestPosition = new double[universe.Size.Length];
            ChangeParameter = universe is SwarmUniverse _universe ? _universe.SwarmChangeParameter : 1.0;
        }
    }
}
