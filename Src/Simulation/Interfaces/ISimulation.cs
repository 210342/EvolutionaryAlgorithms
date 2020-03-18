using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation.Interfaces
{
    interface ISimulation<OrganismType> 
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        IResult Run(IUniverse<IPopulation<OrganismType>, OrganismType> universe, Predicate<IStopCriteria> stopCondition);
    }
}
