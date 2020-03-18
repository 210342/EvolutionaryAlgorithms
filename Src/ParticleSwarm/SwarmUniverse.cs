using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.ParticleSwarm
{
    class SwarmUniverse : IUniverse<IPopulation<Particle>, Particle>
    {
        public Random RNG { get; } = new Random();
        public double[] Size { get; }
        public Func<IEnumerable<double>, double> ApproximatedFunction { get; }
        public Func<double, double, bool> FitnessFunction 
            => new Func<double, double, bool>((left, right) => left < right);

        public double ParticleChangeParameter { get; }
        public double SwarmChangeParameter { get; }
        public IPopulation<Particle> Population => Swarm;
        public Swarm Swarm { get; }

        public SwarmUniverse(
            double[] size, 
            Func<IEnumerable<double>, double> approximatedFunction,
            double particleChangeParameter,
            double swarmChangeParameter,
            int populationSize
            )
        {
            Size = size;
            ApproximatedFunction = approximatedFunction;
            ParticleChangeParameter = particleChangeParameter;
            SwarmChangeParameter = swarmChangeParameter;
            Swarm = new Swarm(this, populationSize);
        }
    }
}
