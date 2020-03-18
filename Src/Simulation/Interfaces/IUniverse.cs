using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.Simulation.Interfaces
{
    public interface IUniverse<PopulationType, OrganismType>
        where PopulationType : IPopulation<OrganismType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        Random RNG { get; }
        double[] Size { get; }
        Func<IEnumerable<double>, double> ApproximatedFunction { get; }
        Func<double, double, bool> FitnessFunction { get; }
        PopulationType Population { get; }

        double[] GenerateRandomVector() => Enumerable.Range(0, Size.Length)
            .Select(i => RNG.NextDouble() * Size[i])
            .ToArray();

        double[] GenerateRandomVector(double maxValue) => Enumerable.Range(0, Size.Length)
            .Select(i => RNG.NextDouble() * maxValue)
            .ToArray();
    }
}
