using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ParticleSwarm
{
    public sealed class Swarm : Population<Particle>
    {
        private readonly double[] _bestPosition;

        public double BestValue { get; set; }
        public double[] BestPosition { get => _bestPosition; set => value.CopyTo(_bestPosition.AsSpan()); }
        public double ChangeRate { get; private set; }
        internal SwarmUniverse SwarmUniverse => Universe as SwarmUniverse;
        public override object Result => BestPosition;

        internal Swarm(IUniverse<IPopulation<Particle>, Particle> universe, uint populationSize) :
            base(universe, Enumerable.Range(0, (int)populationSize).Select(i => new Particle(universe)))
        {
            Particle best = Organisms.Aggregate((p1, p2) => universe.FitnessFunction(p1.BestValue, p2.BestValue) ? p1 : p2);
            _bestPosition = best.BestPosition.Clone() as double[];
            BestValue = best.BestValue;
            ChangeRate = universe is SwarmUniverse _universe ? _universe.Parameters.SwarmChangeRate : 1.0;
        }

        public override void Evolve()
        {
            base.Evolve();
            for (int i = 0; i < Organisms.Length; ++i)
            {
                Organisms[i] = Organisms[i].Evolve(this);
            }
            ChangeRate *= SwarmUniverse.Parameters.DecelerationRate;
        }
    }
}
