using Evo.ParticleSwarm.Experiment;
using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class MultiSwarmUniverse : Universe<Particle, IPopulation<Particle>>, IUniverse<IPopulation<Particle>, Particle>
    {

        public SwarmParameters Parameters { get; set; }

        public override IPopulation<Particle> Population { get; private set; }

        public override double Accuracy => throw new NotImplementedException();

        public MultiSwarmUniverse(
            SwarmParameters swarmParameters,
            Simulation.Range[] universeSize,
            (Func<double[], double>, Func<double, double, bool>) functions)
            : base(universeSize, swarmParameters.MaxEpoch, swarmParameters.MinAccuracy, functions)
        {
            Parameters = swarmParameters;
            Population = new MultiSwarm(this, swarmParameters);
        }
    }
}
