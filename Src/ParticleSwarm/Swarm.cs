using Evo.ParticleSwarm.Experiment;
using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Linq;

namespace Evo.ParticleSwarm
{
    public class Swarm : Population<Particle>
    {
        private readonly double[] _bestPosition;
        private double _bestValue;

        public double BestValue
        {
            get => _bestValue;
            set
            {
                PreviousBestValue = _bestValue;
                _bestValue = value;
            }
        }
        public double PreviousBestValue { get; private set; }
        public bool BestValueChanged { get; private set; } = false;

        public double[] BestPosition { get => _bestPosition; set => value.CopyTo(_bestPosition.AsSpan()); }
        public double ChangeRate { get; private set; } = 1.0;
        public double ChangeAddend { get; } = 0.0;
        internal SwarmParameters SwarmParameters { get; }
        public override object Result => BestPosition;

        internal Swarm(IUniverse<IPopulation<Particle>, Particle> universe, Particle[] particles) : base(universe, particles)
        {
            Particle best = Organisms.Aggregate((p1, p2) => universe.FitnessFunction(p1.BestValue, p2.BestValue) ? p1 : p2);
            _bestPosition = best.BestPosition.Clone() as double[];
            BestValue = best.BestValue;
            PreviousBestValue = BestValue;
            if (universe is SwarmUniverse swarmUniverse)
            {
                ChangeRate = swarmUniverse.Parameters.SwarmChangeRate;
                ChangeAddend = swarmUniverse.Parameters.SwarmChangeAddend;
                SwarmParameters = swarmUniverse.Parameters;
            }
        }

        internal Swarm(IUniverse<IPopulation<Particle>, Particle> universe, uint populationSize) :
            this(universe, Enumerable.Range(0, (int)populationSize).Select(i => new Particle(universe)).ToArray()) { }

        public override void Evolve()
        {
            base.Evolve();

            // evolve particles
            for (int i = 0; i < Organisms.Length; ++i)
            {
                Organisms[i] = Organisms[i].Evolve(this);
            }

            // update parameter
            ChangeRate = Math.Max(0, ChangeRate * SwarmParameters.DecelerationRate + ChangeAddend);

            // update swarm best
            Particle bestInIteration = Organisms.Aggregate((p1, p2) => Universe.FitnessFunction(p1.Value, p2.Value) ? p1 : p2);
            if (Universe.FitnessFunction(bestInIteration.Value, BestValue))
            {
                BestValue = bestInIteration.Value;
                BestPosition = bestInIteration.Position;
                BestValueChanged = true;
            }
            else
            {
                BestValueChanged = false;
            }
        }

        public override bool CanEvolve() => base.CanEvolve() 
            && (ChangeRate > Universe.MinAccuracy || Organisms.Any(o => o.ChangeRate > Universe.MinAccuracy));
    }
}
