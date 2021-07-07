using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ButterflyOptimisation
{
    public class SubSwarm : Population<Butterfly>
    {
        public Butterfly BestButterfly { get; private set; }
        public Butterfly GeneticButterfly { get; private set; }

        public SubSwarm(IUniverse<IPopulation<Butterfly>, Butterfly> universe, IEnumerable<Butterfly> organisms)
            : base(universe, organisms) 
        {
            BestButterfly = Organisms.Aggregate((b1, b2) => Universe.FitnessFunction(b1.Result, b2.Result) ? b1 : b2);
            GeneticButterfly = BestButterfly.Evolve();
        }

        public override object Result => BestButterfly.Position;
        internal double BestFitness => BestButterfly.Result;

        public override void Evolve()
        {
            base.Evolve();

            for (int i = 0; i < Organisms.Length; ++i)
            {
                Organisms[i].Evolve(this);
            }

            BestButterfly = Organisms.Aggregate((b1, b2) => Universe.FitnessFunction(b1.Result, b2.Result) ? b1 : b2);
        }
    }
}
