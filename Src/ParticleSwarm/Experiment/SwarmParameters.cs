﻿using Evo.Simulation.SimulationExperiment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Evo.ParticleSwarm.Experiment
{
    public class SwarmParameters : InputParameters
    {
        public double ParticleChangeRate { get; set; }
        public double ParticleChangeAddend { get; set; }
        public int SwarmPermutePeriod { get; set; } = 0;
        public int SubSwarmCount { get; set; } = 1;
        public double SwarmChangeRate { get; set; }
        public double SwarmChangeAddend { get; set; }
        public double DecelerationRate { get; set; }
        public double InertiaWeight { get; set; }
        public double InertiaWeightRate { get; set; }
        public double InertiaWeightAddend { get; set; }
        public bool UseEliteParticles { get; set; } = false;

        public SwarmParameters() { }

        public SwarmParameters(SwarmParameters other) : base(other)
        {
            foreach (PropertyInfo prop in typeof(SwarmParameters).GetProperties())
            {
                prop.SetValue(this, prop.GetValue(other));
            }
        }

        public override string ToString()
        {
            return string.Join(";", typeof(SwarmParameters).GetProperties().Select(p => p.GetValue(this)));
        }
    }
}
