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
        public double InertiaWeight { get; set; }
        public double InertiaWeightRate { get; set; }
        public double MaxParticleChangeRate { get; set; }
        public double MaxSwarmChangeRate { get; set; }
        public double MaxDecelerationRate { get; set; }
        public double MaxInertiaWeight { get; set; }
        public double MaxInertiaWeightRate { get; set; }
    }
}
