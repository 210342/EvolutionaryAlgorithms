using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class SwarmUniverse : Universe<Particle, IPopulation<Particle>>, IUniverse<IPopulation<Particle>, Particle>
    {
        private double _accuracy;
        private double _previousAccuracy;

        public SwarmParameters Parameters { get; }
        public override IPopulation<Particle> Population => Swarm;
        public Swarm Swarm { get; }
        public override double Accuracy 
        {
            get
            {
                if (Swarm.BestValueChanged)
                {
                    _previousAccuracy = _accuracy;
                    _accuracy = Math.Abs(Swarm.BestValue - Swarm.PreviousBestValue);
                    return _accuracy;
                }
                return _previousAccuracy;
            }
        } 

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
