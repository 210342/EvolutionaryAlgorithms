using Evo.Simulation.Interfaces;
using Evo.Simulation.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evo.ParticleSwarm
{
    public sealed class Particle : Organism<Particle>
    {
        private readonly SwarmUniverse _universe;
        private double[] bestPosition;

        public double[] Velocity { get; private set; }
        public double[] Position { get; private set; }
        public double Value { get; private set; }

        public double BestValue { get; private set; }
        public double[] BestPosition
        {
            get => bestPosition;
            private set => value.CopyTo(BestPosition.AsSpan());
        }

        public double ChangeRate { get; private set; }
        public double InertiaWeight { get; private set; }
        public double InertiaWeightRate { get; }


        #region Initialisation

        internal Particle(IUniverse<IPopulation<Particle>, Particle> universe) : base(universe) 
        {
            _universe = universe as SwarmUniverse;
            ChangeRate = _universe.Parameters.ParticleChangeRate;
            InertiaWeight = _universe.Parameters.InertiaWeight;
            InertiaWeightRate = _universe.Parameters.InertiaWeightRate;
        }

        protected override void InitialiseRandomly(IUniverse<IPopulation<Particle>, Particle> universe)
        {
            Position = universe.GenerateRandomVector();
            Velocity = universe.GenerateRandomVector();
            Value = universe.ApproximatedFunction(Position);
            BestValue = Value;
            bestPosition = new double[universe.Size.Length];
            BestPosition = Position;
        }

        #endregion

        #region Evolution

        public override Particle Evolve() => this;

        public override Particle Evolve(Particle other) => this;

        public override Particle Evolve(IEnumerable<Particle> others) => this;

        public override Particle Evolve(IPopulation<Particle> population) => Evolve(population as Swarm);

        internal Particle Evolve(Swarm swarm)
        {
            // update particle best
            if (swarm.Universe.FitnessFunction(Value, BestValue))
            {
                BestValue = Value;
                BestPosition = Position;
            }

            // update swarm best
            if (swarm.Universe.FitnessFunction(Value, swarm.BestValue))
            {
                swarm.BestValue = Value;
                swarm.BestPosition = BestPosition;
            }

            // update velocity
            double[] cognitive = swarm.Universe.GenerateRandomVector(ChangeRate);
            double[] social = swarm.Universe.GenerateRandomVector(swarm.ChangeRate);
            for (int i = 0; i < Velocity.Length; ++i)
            {
                Velocity[i] = Velocity[i] * InertiaWeight
                    + cognitive[i] * (BestPosition[i] - Position[i])
                    + social[i] * (swarm.BestPosition[i] - Position[i]);
            }

            // update position
            for (int i = 0; i < Position.Length; ++i)
            {
                Position[i] = Math.Min(
                    Math.Max(
                        Position[i] + Velocity[i], 
                        _universe.Size[i].Min
                    ), 
                    _universe.Size[i].Max
                );
            }
            InertiaWeight *= InertiaWeightRate;
            ChangeRate *= _universe.Parameters.DecelerationRate;
            return this;
        }

        #endregion
    }
}
