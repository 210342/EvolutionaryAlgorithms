using Evo.Simulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ParticleSwarm.Experiment
{
    public class SwarmParameters : InputParameters
    {
        public double ParticleChangeRate { get; set; }
        public double SwarmChangeRate { get; set; }
        public double DecelerationRate { get; set; }
        public double InertiaWeight { get; set; }
        public double InertiaWeightRate { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()};{ParticleChangeRate};{SwarmChangeRate};{DecelerationRate};{InertiaWeight};{InertiaWeightRate}";
        }
    }
}
