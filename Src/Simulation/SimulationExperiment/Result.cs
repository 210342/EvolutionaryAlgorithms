using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation.SimulationExperiment
{
    public class Result
    {
        public object ReturnedValue { get; set; }
        public uint Epochs { get; set; }
        public double? Accuracy { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }
}
