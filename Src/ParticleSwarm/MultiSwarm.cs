using Evo.ParticleSwarm.Experiment;
using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class MultiSwarm : Population<Particle>
    {
        protected SubSwarm[] _swarms;

        public IReadOnlyCollection<Swarm> SubSwarms => _swarms as IReadOnlyCollection<Swarm>;
        public SwarmParameters Parameters { get; private set; }


        public MultiSwarm(IUniverse<IPopulation<Particle>, Particle> universe, SwarmParameters parameters) : base(universe)
        {
            Parameters = parameters;
            Organisms = Enumerable
                .Range(0, parameters.SubSwarmCount * (int)parameters.PopulationSize)
                .Select(i => new Particle(universe))
                .ToArray();
            AssignOrganisms(Organisms);
        }

        public override object Result => _swarms.Aggregate((p1, p2) => Universe.FitnessFunction(p1.BestValue, p2.BestValue) ? p1 : p2).BestPosition;

        public override void Evolve()
        {
            base.Evolve();
            if (Parameters.SwarmPermutePeriod != 0 && Epoch % Parameters.SwarmPermutePeriod == 0)
            {
                var shuffled = Organisms.OrderBy(o => Universe.RNG.Next());
                AssignOrganisms(shuffled);
            }
            if (Parameters.UseEliteParticles)
            {
                for (int i = 0; i < Parameters.SubSwarmCount; ++i)
                {
                    for (int dimension = 0; dimension < _swarms[0].BestPosition.Length; ++dimension)
                    {
                        double sum = 0;
                        for (int j = 0; j < Parameters.SubSwarmCount; ++j)
                        {
                            if (i != j)
                            {
                                sum += _swarms[i].BestPosition[dimension];
                            }
                        }
                        _swarms[i].EliteParticle.Position[dimension] = (sum / Parameters.SubSwarmCount) * (1 + Universe.RNG.NextDouble());
                    }
                }
            }
        }

        private void AssignOrganisms(IEnumerable<Particle> organisms)
        {
            for (int i = 0; i < Parameters.SubSwarmCount; ++i)
            {
                _swarms[i] = new SubSwarm(
                    Universe, 
                    organisms
                        .Skip(i * (int)Parameters.PopulationSize)
                        .Take((int)Parameters.PopulationSize)
                        .ToArray()
                );
            }
        }
    }
}
