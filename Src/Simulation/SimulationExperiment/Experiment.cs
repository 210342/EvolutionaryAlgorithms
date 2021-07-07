using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Evo.Simulation.SimulationExperiment
{
    public abstract class Experiment<OrganismType, PopulationType, ParametersType> : IExperiment<ParametersType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
        where PopulationType : IPopulation<OrganismType>
    {
        public ILogger Logger { get; set; }

        public Experiment(ILogger logger)
        {
            Logger = logger;
        }

        public static Function[] Functions { get; } = new Function[]
        {
            new Function(
                Enumerable.Range(0, 20).Select(i => new Range(-50, 50)).ToArray(),
                (double[] x) => x.Aggregate(0, (double x, double y) => x + y * y),
                (y1, y2) => y1 < y2,
                0.0,
                Enumerable.Repeat(0.0, 20).ToArray()
            ),
            new Function(
                Enumerable.Range(0, 30).Select(i => new Range(-50, 50)).ToArray(),
                (double[] x) => x.Aggregate(0, (double x, double y) => x + y * y),
                (y1, y2) => y1 < y2,
                0.0,
                Enumerable.Repeat(0.0, 30).ToArray()
            ),
            new Function(
                Enumerable.Range(0, 20).Select(i => new Range(-100, 100)).ToArray(),
                (double[] x) => Enumerable.Range(0, 20).Select(i => (x[i] - i) * (x[i] - i)).Sum(),
                (y1, y2) => y1 < y2,
                0.0,
                Enumerable.Range(0, 20).Select(i => (double)i).ToArray()
            ),
            new Function(
                Enumerable.Range(0, 30).Select(i => new Range(-100, 100)).ToArray(),
                (double[] x) => Enumerable.Range(0, 30).Select(i => (x[i] - i) * (x[i] - i)).Sum(),
                (y1, y2) => y1 < y2,
                0.0,
                Enumerable.Range(0, 30).Select(i => (double)i).ToArray()
            )
        };

        public static IDictionary<string, Func<IUniverse<IPopulation<OrganismType>, OrganismType>, bool>> StopConditions { get; } =
            new Dictionary<string, Func<IUniverse<IPopulation<OrganismType>, OrganismType>, bool>>()
            {
                {"Iterations", universe => universe.Epoch >= universe.MaxEpoch },
                {"Accuracy", universe => universe.Accuracy < universe.MinAccuracy },
                {"Iterations or Accuracy", universe => (universe.Epoch >= universe.MaxEpoch) || (universe.Accuracy < universe.MinAccuracy) },
                {"Accuracy or Iterations", universe => (universe.Epoch >= universe.MaxEpoch) || (universe.Accuracy < universe.MinAccuracy) }
            };

        public abstract Task Run(ParametersType parameters, StreamWriter output, (int, Function) function);

        protected IEnumerable<double> MeanByIndex(IEnumerable<IList<double[]>> metric)
        {
            IList<double> result = new List<double>();
            for (int i = 0; i < metric.FirstOrDefault().Count; ++i)
            {
                double sum = 0;

                foreach (var list in metric)
                {
                    if (!double.IsNaN(list[i][0]))
                    {
                        sum += list[i][0];
                    }
                }

                result.Add(sum / metric.FirstOrDefault().Count);
            }
            return result;
        }

        protected async Task PrintMetrics(StreamWriter output, List<IDictionary<MetricsEnum, IList<double[]>>> metrics)
        {
            await output.WriteLineAsync();
            await output.WriteAsync("Iterations;");
            await output.WriteLineAsync(string.Join(';', Enumerable.Range(1, 1 + metrics[0][MetricsEnum.ACCURACY_RANGE_KEY].Count)));
            await output.WriteAsync("Minimum accuracy;");
            await output.WriteLineAsync(string.Join(';', metrics[0][MetricsEnum.ACCURACY_RANGE_KEY].Select(d => d[0])));
            await output.WriteAsync("Maximum accuracy;");
            await output.WriteLineAsync(string.Join(';', metrics[0][MetricsEnum.ACCURACY_RANGE_KEY].Select(d => d[1])));
            await output.WriteLineAsync();
            await output.WriteLineAsync($"Standard deviation: ;{CalculateStandardDeviation(metrics)}");
            await output.WriteLineAsync();
            await output.WriteAsync("Iterations;");
            await output.WriteLineAsync(string.Join(';', Enumerable.Range(1, 1 + metrics[0][MetricsEnum.BEST_ACCURACY_KEY].Count)));
            await output.WriteAsync("Mean accuracy;");
            await output.WriteLineAsync(string.Join(';', MeanByIndex(metrics.Select(d => d[MetricsEnum.BEST_ACCURACY_KEY]))));
            await output.WriteLineAsync();
            await output.WriteAsync("Iterations;");
            await output.WriteLineAsync(string.Join(';', Enumerable.Range(1, 1 + metrics[0][MetricsEnum.FITNESS_CHANGE_KEY].Count)));
            await output.WriteAsync("Absolute fitness change between iterations;");
            await output.WriteLineAsync(string.Join(';', MeanByIndex(metrics.Select(d => d[MetricsEnum.FITNESS_CHANGE_KEY]))));
            await output.WriteLineAsync();
        }

        protected async Task PrintAverageTime(StreamWriter output, IList<long> times)
        {
            await output.WriteLineAsync();
            await output.WriteLineAsync($"Average time execution [ms]: ;{times.Average()}");
            await output.WriteLineAsync();
        }

        protected double CalculateStandardDeviation(List<IDictionary<MetricsEnum, IList<double[]>>> metrics)
        {
            double[] fitnesses = metrics.Select(d => d[MetricsEnum.ACCURACY_RANGE_KEY].Select(s => s[0]).LastOrDefault()).ToArray();
            if (fitnesses.Length > 1)
            {
                double average = fitnesses.Average();
                return Math.Sqrt(fitnesses.Sum(d => (d - average) * (d - average)) / fitnesses.Length);
            }
            return 0;
        }
    }
}
