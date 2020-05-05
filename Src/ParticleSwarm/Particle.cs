using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;

namespace Evo.ParticleSwarm
{
    public sealed class Particle : Organism<Particle>
    {
        private readonly SwarmUniverse _universe;
        private double[] _bestPosition;

        public double[] Velocity { get; private set; }
        public double[] Position { get; private set; }
        public double Value { get; private set; }
        public override double Result => Value;

        public double BestValue { get; private set; }
        public double[] BestPosition
        {
            get => _bestPosition;
            private set => value.CopyTo(BestPosition.AsSpan());
        }

        public double ChangeRate { get; private set; }
        public double ChangeAddend { get; }
        public double InertiaWeight { get; private set; }
        public double InertiaWeightRate { get; }
        public double InertiaWeightAddend { get; }
        public double ApplyBestPositionThreshold { get; set; } = 1.0;


        #region Initialisation

        internal Particle(IUniverse<IPopulation<Particle>, Particle> universe) : base(universe) 
        {
            _universe = universe as SwarmUniverse;
            ChangeRate = _universe.Parameters.ParticleChangeRate;
            InertiaWeight = _universe.Parameters.InertiaWeight;
            InertiaWeightRate = _universe.Parameters.InertiaWeightRate;
            InertiaWeightAddend = _universe.Parameters.InertiaWeightAddend;
            ChangeAddend = _universe.Parameters.ParticleChangeAddend;
        }

        protected override void InitialiseRandomly(IUniverse<IPopulation<Particle>, Particle> universe)
        {
            Position = universe.GenerateRandomVector();
            Velocity = universe.GenerateRandomVector();
            Value = universe.ApproximatedFunction(Position);
            BestValue = Value;
            _bestPosition = new double[universe.Size.Length];
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
            return ApplyBestPositionThreshold >= 1.0 || _universe.RNG.NextDouble() < ApplyBestPositionThreshold
                ? EvolveByVelocity(swarm)
                : EvolveByBestPosition(swarm);
        }

        private Particle EvolveByVelocity(Swarm swarm)
        {
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

            // update parameters
            InertiaWeight = Math.Max(0, InertiaWeight * InertiaWeightRate + InertiaWeightAddend);
            ChangeRate = Math.Max(0, ChangeRate * _universe.Parameters.DecelerationRate + ChangeAddend);

            Value = swarm.Universe.ApproximatedFunction(Position);

            // update particle best
            if (swarm.Universe.FitnessFunction(Value, BestValue))
            {
                BestValue = Value;
                BestPosition = Position;
            }

            return this;
        }

        private Particle EvolveByBestPosition(Swarm swarm)
        {
            for (int i = 0; i < Position.Length; ++i)
            {
                Position[i] = BestPosition[i] * (1 + _universe.RNG.NextDouble());
            }
            return this; 
        }

        #endregion
    }
}
