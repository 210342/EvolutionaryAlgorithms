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
    public class MultiSwarmExperiment : Experiment<Particle, IPopulation<Particle>, SwarmConfig>
    {
        public MultiSwarmExperiment(ILogger logger) : base(logger) { }

        public override async Task Run(SwarmConfig config, StreamWriter output, (int, Function) functionTuple)
        {
            int functionIndex = functionTuple.Item1;
            Function function = functionTuple.Item2;

            var mpsoMetrics = new List<IDictionary<string, IList<double[]>>>();
            var mspsoMetrics = new List<IDictionary<string, IList<double[]>>>();
            for (uint populationSize = config.ExperimentParameters.Min.PopulationSize;
                populationSize <= config.ExperimentParameters.Max.PopulationSize;
                populationSize += config.ExperimentParameters.Rate.PopulationSize)
            {
                Logger.Print($"Population {populationSize}");
                await output.WriteLineAsync($"MPSO;function: ;{functionIndex};Population: ;{populationSize}; Experiments: ;{config.ExperimentsCount}");
                for (int i = 0; i < config.ExperimentsCount; ++i)
                {
                    config.SwarmParameters.PopulationSize = populationSize;
                    MultiSwarmUniverse universe = new MultiSwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                    Result result = new Simulation<Particle>().Run(
                        universe,
                        StopConditions[config.SwarmParameters.StopCondition]
                    );
                    mpsoMetrics.Add(universe.Metrics);
                }
                await output.WriteLineAsync(string.Join(';', Enumerable.Range(1, 1 + mpsoMetrics[0][MultiSwarmUniverse.ACCURACY_RANGE_KEY].Count)));
                await output.WriteLineAsync(string.Join(';', mpsoMetrics[0][MultiSwarmUniverse.ACCURACY_RANGE_KEY].Select(d => d[0])));
                await output.WriteLineAsync(string.Join(';', mpsoMetrics[0][MultiSwarmUniverse.ACCURACY_RANGE_KEY].Select(d => d[1])));
                await output.WriteLineAsync();
                await output.WriteLineAsync(string.Join(';', Enumerable.Range(1, 1 + mpsoMetrics[0][MultiSwarmUniverse.MEAN_ACCURACY_KEY].Count)));
                await output.WriteLineAsync(string.Join(';', MeanByIndex(mpsoMetrics.Select(d => d[MultiSwarmUniverse.MEAN_ACCURACY_KEY]))));


                await output.WriteLineAsync($"MSPSO;function: ;{functionIndex};Population: ;{populationSize}; Experiments: ;{config.ExperimentsCount}");
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
    }
}
