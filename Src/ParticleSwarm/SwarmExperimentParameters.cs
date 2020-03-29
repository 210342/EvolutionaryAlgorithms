using Evo.Simulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class SwarmExperimentParameters : ExperimentParameters
    {
        public double ParticleChangeRate { get; set; }
        public double SwarmChangeRate { get; set; }
        public double DecelerationRate { get; set; }
        public double VelocityRate { get; set; }
        public double MaxParticleChangeRate { get; set; }
        public double MaxSwarmChangeRate { get; set; }
        public double MaxDecelerationRate { get; set; }
        public double MaxVelocityRate { get; set; }
    }
}
