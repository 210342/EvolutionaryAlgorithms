using Evo.Simulation.Interfaces;
using System.Collections.Generic;

namespace Evo.Simulation.Abstracts
{
    public abstract class Organism<OrganismType> : IOrganism<OrganismType, IPopulation<OrganismType>>
        where OrganismType : Organism<OrganismType>
    {
        public Organism(IUniverse<IPopulation<OrganismType>, OrganismType> universe)
        {
            InitialiseRandomly(universe);
        }

        public abstract OrganismType Evolve();

        public abstract OrganismType Evolve(OrganismType other);

        public abstract OrganismType Evolve(IEnumerable<OrganismType> others);

        public abstract OrganismType Evolve(IPopulation<OrganismType> population);

        protected abstract void InitialiseRandomly(IUniverse<IPopulation<OrganismType>, OrganismType> universe);
    }
}
