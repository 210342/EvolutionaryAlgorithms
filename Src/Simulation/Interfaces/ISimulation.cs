using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation.Interfaces
{
    public interface ISimulation<OrganismType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        object Run(
            IUniverse<IPopulation<OrganismType>, OrganismType> universe, 
            Predicate<IUniverse<IPopulation<OrganismType>, OrganismType>> stopCondition
        );
    }
}
