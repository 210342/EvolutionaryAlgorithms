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
        public static readonly string ACCURACY_RANGE_KEY = "Accuracy range";
        public static readonly string MEAN_ACCURACY_KEY = "Mean accuracy";

        public Random RNG { get; } = new Random();
        public Range[] Size { get; }
        public Func<double[], double> ApproximatedFunction { get; }
        public abstract PopulationType Population { get; }
        public Func<double, double, bool> FitnessFunction { get; }
        public uint MaxEpoch { get; }
        public uint Epoch => Population.Epoch;
        public abstract double Accuracy { get; }
        public double MinAccuracy { get; }
        public IDictionary<string, IList<double[]>> Metrics { get; } = new Dictionary<string, IList<double[]>>();

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
        }

        public virtual void IterateEpoch()
        {
            Population.Evolve();
            EpochElapsed?.Invoke(this);
        }

        private void AccuracyRangeMetric(Universe<OrganismType, PopulationType> universe)
        {
            if (!Metrics.ContainsKey(ACCURACY_RANGE_KEY))
            {
                Metrics.Add(ACCURACY_RANGE_KEY, new List<double[]>());
            }
            Metrics[ACCURACY_RANGE_KEY].Add(new[] { ApproximatedFunction(Population.Result as double[]) });
        }

        private void MeanAccuracyMetric(Universe<OrganismType, PopulationType> universe)
        {
            if (!Metrics.ContainsKey(MEAN_ACCURACY_KEY))
            {
                Metrics.Add(MEAN_ACCURACY_KEY, new List<double[]>());
            }
            double min = Population.Organisms.Aggregate((p1, p2) => FitnessFunction(p1.Result, p2.Result) ? p1 : p2).Result;
            double max = Population.Organisms.Aggregate((p1, p2) => FitnessFunction(p1.Result, p2.Result) ? p2 : p1).Result;
            Metrics[MEAN_ACCURACY_KEY].Add(new[] { min, max });
        }
    }
}
