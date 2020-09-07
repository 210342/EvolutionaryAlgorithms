using Evo.Simulation.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Evo.Simulation.Abstracts
{
    public abstract class Universe<OrganismType, PopulationType> : IUniverse<PopulationType, OrganismType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
        where PopulationType : IPopulation<OrganismType>
    {
        public Random RNG { get; } = new Random();
        public Range[] Size { get; }
        public Func<double[], double> ApproximatedFunction { get; }
        public abstract PopulationType Population { get; }
        public Func<double, double, bool> FitnessFunction { get; }
        public uint MaxEpoch { get; }
        public uint Epoch => Population.Epoch;
        public abstract double? Accuracy { get; }
        public double MinAccuracy { get; }
        public IDictionary<MetricsEnum, IList<double[]>> Metrics { get; } = new Dictionary<MetricsEnum, IList<double[]>>();

        public event Action<Universe<OrganismType, PopulationType>> EpochElapsed;

        public Universe(
            Range[] size, 
            uint maxEpoch,
            double minAccuracy,
            (Func<double[], double>, Func<double, double, bool>) functions)
        {
            Size = size;
            MaxEpoch = maxEpoch;
            MinAccuracy = minAccuracy;
            ApproximatedFunction = functions.Item1;
            FitnessFunction = functions.Item2;
            EpochElapsed += AccuracyRangeMetric;
            EpochElapsed += MeanAccuracyMetric;
            EpochElapsed += FitnessChangeMetric;
        }

        public virtual void IterateEpoch()
        {
            Population.Evolve();
            EpochElapsed?.Invoke(this);
        }

        private void AccuracyRangeMetric(Universe<OrganismType, PopulationType> universe)
        {
            if (!Metrics.ContainsKey(MetricsEnum.BEST_ACCURACY_KEY))
            {
                Metrics.Add(MetricsEnum.BEST_ACCURACY_KEY, new List<double[]>());
            }
            Metrics[MetricsEnum.BEST_ACCURACY_KEY].Add(new[] { ApproximatedFunction(Population.Result as double[]) });
        }

        private void MeanAccuracyMetric(Universe<OrganismType, PopulationType> universe)
        {
            if (!Metrics.ContainsKey(MetricsEnum.ACCURACY_RANGE_KEY))
            {
                Metrics.Add(MetricsEnum.ACCURACY_RANGE_KEY, new List<double[]>());
            }
            double min = Population.Organisms.Aggregate((p1, p2) => FitnessFunction(p1.Result, p2.Result) ? p1 : p2).Result;
            double max = Population.Organisms.Aggregate((p1, p2) => FitnessFunction(p1.Result, p2.Result) ? p2 : p1).Result;
            Metrics[MetricsEnum.ACCURACY_RANGE_KEY].Add(new[] { min, max });
        }

        private void FitnessChangeMetric(Universe<OrganismType, PopulationType> universe)
        {
            if (!Metrics.ContainsKey(MetricsEnum.FITNESS_CHANGE_KEY))
            {
                Metrics.Add(MetricsEnum.FITNESS_CHANGE_KEY, new List<double[]>());
            }
            Metrics[MetricsEnum.FITNESS_CHANGE_KEY].Add(new[] { Math.Abs(Population.Fitness - Population.PreviousEpochFitness) });
        }
    }
}
