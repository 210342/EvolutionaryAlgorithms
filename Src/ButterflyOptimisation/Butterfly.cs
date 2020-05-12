using Evo.ButterflyOptimisation.Experiment;
using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Evo.ButterflyOptimisation
{
    public class Butterfly : Organism<Butterfly>
    {
        private readonly IUniverse<IPopulation<Butterfly>, Butterfly> _universe;

        public double[] Position { get; private set; }
        public BoaParameters Parameters { get; }

        public override double Result => _universe.ApproximatedFunction(Position);
        public double Fragrance => Parameters.SensorModality * Math.Pow(Result, Parameters.PowerExponent);


        public Butterfly(IUniverse<IPopulation<Butterfly>, Butterfly> universe, BoaParameters parameters) : base(universe)
        {
            _universe = universe;
            Parameters = parameters;
        }

        protected override void InitialiseRandomly(IUniverse<IPopulation<Butterfly>, Butterfly> universe)
        {
            Position = universe.GenerateRandomVector();
        }

        public override Butterfly Evolve() => this;

        public override Butterfly Evolve(Butterfly other) => this;

        public override Butterfly Evolve(IEnumerable<Butterfly> others) => this;

        public override Butterfly Evolve(IPopulation<Butterfly> population) => Evolve(population as SubSwarm);

        private Butterfly Evolve(SubSwarm swarm)
        {
            double r = _universe.RNG.NextDouble();
            if (r < Parameters.SwitchProbability)
            {
                for (int i = 0; i < Position.Length; ++i)
                {
                    Position[i] += Fragrance * (r * r * swarm.BestButterfly.Position[i] - Position[i]);
                }
            }
            else
            {
                var randomPair = _universe.GetRandomPair(0, swarm.Organisms.Length);
                for (int i = 0; i < Position.Length; ++i)
                {
                    Position[i] += Fragrance *
                        (r * r * swarm.Organisms[randomPair.Item1].Position[i]
                            - swarm.Organisms[randomPair.Item2].Position[i]);
                }
            }
            return this;
        }
    }
}
