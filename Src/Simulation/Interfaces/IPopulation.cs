using System;

namespace Evo.Simulation.Interfaces
{
    public interface IPopulation<OrganismType> 
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        IUniverse<IPopulation<OrganismType>, OrganismType> Universe { get; }
        uint Epoch { get; }
        object Result { get; }
        OrganismType[] Organisms { get; }
        double PreviousEpochFitness { get; }
        double Fitness { get; }

        void Evolve();
        bool CanEvolve();
    }
}
