using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.Simulation
{
    public class ExperimentParameters
    {
        public uint MinEpoch { get; set; }
        public uint MinPopulation { get; set; }
        public uint EpochRate { get; set; }
        public uint PopulationRate { get; set; }
        public uint MaxEpoch { get; set; }
        public uint MaxPopulation { get; set; }
    }
}
