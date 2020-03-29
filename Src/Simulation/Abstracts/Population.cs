using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.Simulation.Abstracts
{
    public abstract class Population<OrganismType> : IPopulation<OrganismType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        public IUniverse<IPopulation<OrganismType>, OrganismType> Universe { get; }
        public OrganismType[] Organisms { get; }
        public int PopulationSize => Organisms.Length;
        public uint Epoch { get; protected set; } = 0;

        public abstract object Result { get; }

        public Population(IUniverse<IPopulation<OrganismType>, OrganismType> universe, IEnumerable<OrganismType> organisms)
        {
            Universe = universe;
            Organisms = organisms.ToArray();
        }

        public virtual void Evolve()
        {
            ++Epoch;
        }
    }
}
