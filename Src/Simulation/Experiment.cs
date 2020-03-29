using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation
{
    public abstract class Experiment<OrganismType, PopulationType, ParametersType> : IExperiment<ParametersType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
        where PopulationType : IPopulation<OrganismType>
    {
        protected (Func<double[], double>, Func<double, double, bool>)[] Functions { get; } = 
            new (Func<double[], double>, Func<double, double, bool>)[]
        {
            (x => (x[0] * x[0]) + (x[1] * x[1]), (y1, y2) => y1 < y2)
        };

        protected IDictionary<string, Func<IUniverse<IPopulation<OrganismType>, OrganismType>, bool>> StopConditions { get; } =
            new Dictionary<string, Func<IUniverse<IPopulation<OrganismType>, OrganismType>, bool>>()
            {
                {"Iterations", universe => universe.Epoch >= universe.MaxEpoch }
            };

        public abstract IEnumerable<(object, InputParameters)> Run(ParametersType parameters);
    }
}
