using Evo.Simulation;
using Evo.Simulation.Interfaces;
using Evo.Simulation.SimulationExperiment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.ParticleSwarm.Experiment
{
    public class MultiSwarmExperiment : Experiment<Particle, IPopulation<Particle>, MultiSwarmConfig>
    {
        public MultiSwarmExperiment(ILogger logger) : base(logger) { }

        public override async Task Run(MultiSwarmConfig config, StreamWriter output, (int, Function) functionTuple)
        {
            int functionIndex = functionTuple.Item1;
            Function function = functionTuple.Item2;

            var mpsoParams = new SwarmParameters(config.SwarmConfig.SwarmParameters)
            {
                SwarmPermutePeriod = 8
            };
            var mspsoParams = new SwarmParameters(config.SwarmConfig.SwarmParameters)
            {
                UseEliteParticles = true
            };

            var mpsoMetrics = new List<IDictionary<string, IList<double[]>>>();
            var mspsoMetrics = new List<IDictionary<string, IList<double[]>>>();
            for (uint populationSize = config.SwarmConfig.ExperimentParameters.Min.PopulationSize;
                populationSize <= config.SwarmConfig.ExperimentParameters.Max.PopulationSize;
                populationSize += config.SwarmConfig.ExperimentParameters.Rate.PopulationSize)
            {
                Logger.Print($"Population {populationSize}");
                mpsoParams.PopulationSize = populationSize;
                mspsoParams.PopulationSize = populationSize;

                await output.WriteLineAsync($"MPSO;function: ;{functionIndex};Population: ;{populationSize}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    MultiSwarmUniverse universe = new MultiSwarmUniverse(mpsoParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    new Simulation<Particle>().Run(
                        universe,
                        StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    mpsoMetrics.Add(universe.Metrics);
                }
                await PrintMetrics(output, mpsoMetrics);

                await output.WriteLineAsync($"MSPSO;function: ;{functionIndex};Population: ;{populationSize}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    MultiSwarmUniverse universe = new MultiSwarmUniverse(mspsoParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    new Simulation<Particle>().Run(
                        universe,
                        StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    mspsoMetrics.Add(universe.Metrics);
                }
                await PrintMetrics(output, mspsoMetrics);
            }
        }

        private IEnumerable<double> MeanByIndex(IEnumerable<IList<double[]>> metric)
        {
            IList<double> result = new List<double>();
            for (int i = 0; i < metric.FirstOrDefault().Count; ++i)
            {
                double sum = 0;

                foreach(var list in metric)
                {
                    sum += list[i][0];
                }

                result.Add(sum / metric.FirstOrDefault().Count);
            }
            return result;
        }

        private async Task PrintMetrics(StreamWriter output, List<IDictionary<string, IList<double[]>>>metrics)
        {
            await output.WriteLineAsync();
            await output.WriteAsync("Iterations;");
            await output.WriteLineAsync(string.Join(';', Enumerable.Range(1, 1 + metrics[0][MultiSwarmUniverse.ACCURACY_RANGE_KEY].Count)));
            await output.WriteAsync("Minimum accuracy;");
            await output.WriteLineAsync(string.Join(';', metrics[0][MultiSwarmUniverse.ACCURACY_RANGE_KEY].Select(d => d[0])));
            await output.WriteAsync("Maximum accuracy;");
            await output.WriteLineAsync(string.Join(';', metrics[0][MultiSwarmUniverse.ACCURACY_RANGE_KEY].Select(d => d[1])));
            await output.WriteLineAsync();
            await output.WriteAsync("Iterations;");
            await output.WriteLineAsync(string.Join(';', Enumerable.Range(1, 1 + metrics[0][MultiSwarmUniverse.MEAN_ACCURACY_KEY].Count)));
            await output.WriteAsync("Mean accuracy;");
            await output.WriteLineAsync(string.Join(';', MeanByIndex(metrics.Select(d => d[MultiSwarmUniverse.MEAN_ACCURACY_KEY]))));
            await output.WriteLineAsync();
        }
    }
}
