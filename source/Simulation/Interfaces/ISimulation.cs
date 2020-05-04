using Evo.Simulation.SimulationExperiment;
using System;

namespace Evo.Simulation.Interfaces
{
    public interface ISimulation<OrganismType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        Result Run(
            IUniverse<IPopulation<OrganismType>, OrganismType> universe,
            Func<IUniverse<IPopulation<OrganismType>, OrganismType>, bool> stopCondition
        );
    }
}
