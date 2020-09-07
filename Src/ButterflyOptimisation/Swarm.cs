using Evo.ButterflyOptimisation.Experiment;
using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ButterflyOptimisation
{
    public class Swarm : Population<Butterfly>
    {
        private SubSwarm[] _swarms;

        public BoaParameters Parameters { get; set; }
        public override object Result => _swarms.Aggregate((s1, s2) =>
                Universe.FitnessFunction(s1.BestFitness, s2.BestFitness) ? s1 : s2).Result;

        public Swarm(IUniverse<IPopulation<Butterfly>, Butterfly> universe, BoaParameters parameters)
            : base(
                universe,
                Enumerable
                    .Range(0, parameters.SubPopulationCount)
                    .SelectMany(i => Enumerable
                        .Range(0, (int)parameters.PopulationSize)
                        .Select(j => new Butterfly(universe, parameters))))
        {
            Parameters = parameters;
            CreateSubSwarms(Organisms);
        }

        public Swarm(IUniverse<IPopulation<Butterfly>, Butterfly> universe, BoaParameters parameters, double[][] templateParticles) 
            : base(universe)
        {
            Parameters = parameters;
            CreateSubSwarms(templateParticles);
            Organisms = _swarms.SelectMany(s => s.Organisms).ToArray();
        }

        private void CreateSubSwarms(IEnumerable<Butterfly> organisms)
        {
            _swarms = Enumerable.Range(0, Parameters.SubPopulationCount)
                .Select(i => new SubSwarm(
                    Universe,
                    organisms
                        .Skip(i * (int)Parameters.PopulationSize)
                        .Take((int)Parameters.PopulationSize)
                        .ToArray()
                ))
                .ToArray();
        }

        private void CreateSubSwarms(double[][] templateParticles)
        {
            _swarms = templateParticles
                .Select(templatePosition =>
                    new SubSwarm(
                    Universe,
                    Enumerable.Range(0, (int)Parameters.PopulationSize - 1)
                        .Select(i => new Butterfly(Universe, Parameters))
                        .Concat(new[] { new Butterfly(Universe, Parameters, templatePosition) })
                ))
                .ToArray();
        }

        private void AssignOrganisms(IEnumerable<Butterfly> organisms)
        {
            for (int i = 0; i < Parameters.SubPopulationCount; ++i)
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

        public override void Evolve()
        {
            base.Evolve();

            for (int i = 0; i < _swarms.Length; ++i)
            {
                _swarms[i].Evolve();
            }

            if (Epoch % Parameters.SwarmPermutePeriod == 0)
            {
                AssignOrganisms(Organisms.OrderBy(o => Universe.RNG.Next()));
            }
        }
    }
}
