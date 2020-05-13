using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evo.Simulation.Abstracts
{
    public abstract class Population<OrganismType> : IPopulation<OrganismType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        public IUniverse<IPopulation<OrganismType>, OrganismType> Universe { get; }
        public OrganismType[] Organisms { get; protected set; }
        public int PopulationSize => Organisms.Length;
        public uint Epoch { get; protected set; } = 0;

        public double PreviousEpochFitness { get; private set; }
        public double Fitness => Universe.ApproximatedFunction(Result as double[]);
        public abstract object Result { get; }

        protected Population(IUniverse<IPopulation<OrganismType>, OrganismType> universe)
        {
            Universe = universe;
        }

        public Population(IUniverse<IPopulation<OrganismType>, OrganismType> universe, IEnumerable<OrganismType> organisms)
        {
            Universe = universe;
            Organisms = organisms.ToArray();
        }

        public virtual void Evolve()
        {
            PreviousEpochFitness = Universe.ApproximatedFunction(Result as double[]);
            ++Epoch;
        }

        public virtual bool CanEvolve() => true;
    }
}
