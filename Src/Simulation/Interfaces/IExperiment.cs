using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation.Interfaces
{
    public interface IExperiment<ParametersType>
    {
        IEnumerable<(object, InputParameters)> Run(ParametersType parameters);
    }
}
