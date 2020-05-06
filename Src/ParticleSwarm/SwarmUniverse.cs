using Evo.ParticleSwarm.Experiment;
using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;

namespace Evo.ParticleSwarm
{
    public class SwarmUniverse : Universe<Particle, IPopulation<Particle>>, IUniverse<IPopulation<Particle>, Particle>
    {
        public SwarmParameters Parameters { get; }
        public override IPopulation<Particle> Population => Swarm;
        public Swarm Swarm { get; protected set; }
        public override double? Accuracy => Swarm?.BestValueChanged ?? false 
            ? Math.Abs(Swarm?.BestValue - Swarm?.PreviousBestValue ?? 0) 
            : Swarm?.ChangeRate;

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
