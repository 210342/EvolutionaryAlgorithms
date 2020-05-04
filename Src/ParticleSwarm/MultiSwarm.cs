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
        protected IEnumerable<Swarm> _swarms;

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

        public override object Result => throw new NotImplementedException();

        public override void Evolve()
        {
            base.Evolve();
            if (Parameters.SwarmPermutePeriod != 0 && Epoch % Parameters.SwarmPermutePeriod == 0)
            {
                var shuffled = Organisms.OrderBy(o => Universe.RNG.Next());
                AssignOrganisms(shuffled);
            }
        }

        private void AssignOrganisms(IEnumerable<Particle> organisms)
        {
            _swarms = Enumerable
                .Range(0, Parameters.SubSwarmCount)
                .Select(i =>
                    new SubSwarm(Universe, organisms.Skip(i * (int)Parameters.PopulationSize).Take((int)Parameters.PopulationSize).ToArray())
                );
        }
    }
}
