using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class SwarmUniverse : IUniverse<IPopulation<Particle>, Particle>
    {
        public Random RNG { get; } = new Random();
        public Simulation.Range[] Size { get; }
        public Func<double[], double> ApproximatedFunction { get; }
        public Func<double, double, bool> FitnessFunction { get; } =
            new Func<double, double, bool>((left, right) => left < right);

        public SwarmParameters Parameters { get; }
        public IPopulation<Particle> Population => Swarm;
        public Swarm Swarm { get; }

        public SwarmUniverse(
            SwarmParameters swarmParameters,
            Func<double[], double> approximatedFunction,
            Func<double, double, bool> fitnessFunction
            )
        {
            Parameters = swarmParameters;
            Size = swarmParameters.UniverseSize;
            FitnessFunction = fitnessFunction;
            ApproximatedFunction = approximatedFunction;
            Swarm = new Swarm(this, swarmParameters.PopulationSize);
        }
    }
}
