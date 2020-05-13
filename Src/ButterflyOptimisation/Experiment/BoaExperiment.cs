using Evo.Simulation;
using Evo.Simulation.Interfaces;
using Evo.Simulation.SimulationExperiment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Evo.ButterflyOptimisation.Experiment
{
    public class BoaExperiment : Experiment<Butterfly, IPopulation<Butterfly>, BoaConfig>
    {
        public BoaExperiment(ILogger logger) : base(logger)
        {
        }

        public override Task Run(BoaConfig parameters, StreamWriter output, (int, Function) function)
        {
            throw new NotImplementedException();
        }
    }
}
