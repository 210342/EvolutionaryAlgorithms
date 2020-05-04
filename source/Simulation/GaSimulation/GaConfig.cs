using Evo.Simulation.SimulationExperiment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation.GaSimulation
{
    public class GaConfig
    {
        public int FunctionIndex { get; set; }
        public InputParameters InputParameters { get; set; }
        public ExperimentParameters ExperimentParameters { get; set; }
    }
}
