using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation
{
    public class Simulation<OrganismType> : ISimulation<OrganismType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        public object Run(
            IUniverse<IPopulation<OrganismType>, OrganismType> universe,
            Func<IUniverse<IPopulation<OrganismType>, OrganismType>, bool> stopCondition)
        {
            while (!stopCondition(universe))
            {
                universe.Population.Evolve();
            }
            return universe.Population.Result;
        }
    }
}
