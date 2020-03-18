﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation.Interfaces
{
    public interface IPopulation<OrganismType> 
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        IUniverse<IPopulation<OrganismType>, OrganismType> Universe { get; }
        void Evolve();
    }
}
