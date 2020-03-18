using Evo.Simulation.Interfaces;
using Evo.Simulation.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evo.ParticleSwarm
{
    public class Particle : Organism<Particle>
    {
        private double[] bestPosition;

        public double[] Position { get; private set; }
        public double[] Velocity { get; private set; }
        public double Value { get; private set; }
        public double BestValue { get; private set; }
        public double[] BestPosition
        {
            get => bestPosition;
            private set => value.CopyTo(BestPosition.AsSpan());
        }
        public double ChangeParameter { get; private set; }

        #region Initialisation

        internal Particle(IUniverse<IPopulation<Particle>, Particle> universe) : base(universe) { }

        protected override void InitialiseRandomly(IUniverse<IPopulation<Particle>, Particle> universe)
        {
            Position = universe.GenerateRandomVector();
            Velocity = universe.GenerateRandomVector();
            Value = universe.ApproximatedFunction(Position);
            BestValue = Value;
            bestPosition = new double[universe.Size.Length];
            BestPosition = Position;
            ChangeParameter = universe is SwarmUniverse _universe ? _universe.ParticleChangeParameter : 1.0;
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
            double[] particleChange = swarm.Universe.GenerateRandomVector(ChangeParameter);
            double[] swarmChange = swarm.Universe.GenerateRandomVector(swarm.ChangeParameter);
            for (int i = 0; i < Velocity.Length; ++i)
            {
                Velocity[i] += particleChange[i] * (BestPosition[i] - Position[i])
                    + swarmChange[i] * (swarm.BestPosition[i] - Position[i]);
            }

            // update possition
            for (int i = 0; i < Position.Length; ++i)
            {
                Position[i] += Velocity[i];
            }
            return this;
        }

        #endregion
    }
}
