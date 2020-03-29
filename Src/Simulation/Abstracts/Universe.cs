using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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

        public Universe(
            Range[] size, 
            uint maxEpoch,
            (Func<double[], double>, Func<double, double, bool>) functions)
        {
            Size = size;
            MaxEpoch = maxEpoch;
            ApproximatedFunction = functions.Item1;
            FitnessFunction = functions.Item2;
        }

    }
}
