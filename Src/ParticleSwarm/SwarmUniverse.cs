﻿using Evo.ParticleSwarm.Experiment;
using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;

namespace Evo.ParticleSwarm
{
    public class SwarmUniverse : Universe<Particle, IPopulation<Particle>>, IUniverse<IPopulation<Particle>, Particle>
    {
        public SwarmParameters Parameters { get; }
        public override IPopulation<Particle> Population => Swarm;
        public Swarm Swarm { get; }
        public override double Accuracy => Swarm.BestValueChanged 
            ? Math.Abs(Swarm.BestValue - Swarm.PreviousBestValue) 
            : Swarm.ChangeRate;

        public SwarmUniverse(
            SwarmParameters swarmParameters,
            Simulation.Range[] universeSize,
            (Func<double[], double>, Func<double, double, bool>) functions)
            : base(universeSize, swarmParameters.MaxEpoch, swarmParameters.MinAccuracy, functions)
        {
            Parameters = swarmParameters;
            Swarm = new Swarm(this, swarmParameters.PopulationSize);
        }
    }
}
