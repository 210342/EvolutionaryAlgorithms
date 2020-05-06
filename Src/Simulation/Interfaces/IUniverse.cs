using System;
using System.Linq;

namespace Evo.Simulation.Interfaces
{
    public interface IUniverse<PopulationType, OrganismType>
        where PopulationType : IPopulation<OrganismType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        Random RNG { get; }
        Range[] Size { get; }
        Func<double[], double> ApproximatedFunction { get; }
        Func<double, double, bool> FitnessFunction { get; }
        PopulationType Population { get; }
        uint Epoch { get; }
        uint MaxEpoch { get; }
        double? Accuracy { get; }
        double MinAccuracy { get; }

        double[] GenerateRandomVector() => Enumerable.Range(0, Size.Length)
            .Select(i => 0.5 * (RNG.NextDouble() * (Size[i].Max - Size[i].Min) + Size[i].Min))
            .ToArray();

        double[] GenerateRandomVector(double maxValue) => Enumerable.Range(0, Size.Length)
            .Select(i => RNG.NextDouble() * maxValue)
            .ToArray();

        double[] GenerateRandomVector(double minValue, double maxValue) => Enumerable.Range(0, Size.Length)
            .Select(i => RNG.NextDouble() * (maxValue - minValue) + minValue)
            .ToArray();

        void IterateEpoch();
    }
}
