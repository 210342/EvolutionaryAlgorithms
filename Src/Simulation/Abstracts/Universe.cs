using Evo.Simulation.Interfaces;
using System;

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
        public abstract double Accuracy { get; }
        public double MinAccuracy { get; }

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
        }

    }
}
