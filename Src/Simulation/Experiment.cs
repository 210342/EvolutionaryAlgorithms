using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Simulation
{
    public abstract class Experiment<OrganismType, PopulationType, ParametersType> : IExperiment<ParametersType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
        where PopulationType : IPopulation<OrganismType>
    {
        public static Function[] Functions { get; } = new Function[]
        {
            new Function(
                new Range[] { new Range(-50, 50), new Range(-50, 50) },
                x => (x[0] * x[0]) + (x[1] * x[1]), 
                (y1, y2) => y1 < y2, 
                0.0, 
                new[] { 0.0, 0.0 }),
            new Function(
                Enumerable.Range(0, 25).Select(i => new Range(-50, 50)).ToArray(),
                (double[] x) => x.Aggregate(0, (double x, double y) => x + y * y),
                (y1, y2) => y1 < y2,
                0.0,
                Enumerable.Repeat(0.0, 25).ToArray()
            ),
            new Function(
                Enumerable.Range(0, 25).Select(i => new Range(-100, 100)).ToArray(),
                (double[] x) => Enumerable.Range(0, 25).Select(i => (x[i] - i) * (x[i] - i)).Sum(),
                (y1, y2) => y1 < y2,
                0.0,
                Enumerable.Range(0, 25).Select(i => (double)i).ToArray()
            )
        };

        protected IDictionary<string, Func<IUniverse<IPopulation<OrganismType>, OrganismType>, bool>> StopConditions { get; } =
            new Dictionary<string, Func<IUniverse<IPopulation<OrganismType>, OrganismType>, bool>>()
            {
                {"Iterations", universe => universe.Epoch >= universe.MaxEpoch },
                {"Accuracy", universe => universe.MinAccuracy >= universe.Accuracy },
                {"Iterations or Accuracy", universe => (universe.Epoch >= universe.MaxEpoch) || (universe.MinAccuracy <= universe.Accuracy) },
                {"Accuracy or Iterations", universe => (universe.Epoch >= universe.MaxEpoch) || (universe.MinAccuracy <= universe.Accuracy) }
            };

        public abstract Task Run(ParametersType parameters, StreamWriter output, (int, Function) function);
    }
}
