﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation.Interfaces
{
    public interface IOrganism<OrganismType, PopulationType> 
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>> 
        where PopulationType : IPopulation<OrganismType>
    {
        OrganismType Evolve();
        OrganismType Evolve(OrganismType other);
        OrganismType Evolve(IEnumerable<OrganismType> others);
        OrganismType Evolve(PopulationType population);
    }
}
