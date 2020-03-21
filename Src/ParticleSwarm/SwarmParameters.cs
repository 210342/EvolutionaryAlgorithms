using Evo.Simulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class SwarmParameters : InputParameters
    {
        public double ParticleChangeRate { get; set; }
        public double SwarmChangeRate { get; set; }
        public double DecelerationRate { get; set; }
        public double VelocityRate { get; set; }
    }
}
