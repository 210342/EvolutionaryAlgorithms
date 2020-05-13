using Evo.ButterflyOptimisation;
using Evo.ButterflyOptimisation.Experiment;
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
            var boaParams = new BoaParameters(config.BoaConfig.BoaParameters);

            var mpsoMetrics = new List<IDictionary<string, IList<double[]>>>();
            var mspsoMetrics = new List<IDictionary<string, IList<double[]>>>();
            var boaMetrics = new List<IDictionary<string, IList<double[]>>>();
            IList<long> times = new List<long>();
            for (uint populationSize = config.SwarmConfig.ExperimentParameters.Min.PopulationSize;
                populationSize <= config.SwarmConfig.ExperimentParameters.Max.PopulationSize;
                populationSize += config.SwarmConfig.ExperimentParameters.Rate.PopulationSize)
            {
                Logger.Print($"Population {populationSize}");
                mpsoParams.PopulationSize = populationSize;
                mspsoParams.PopulationSize = populationSize;
                boaParams.PopulationSize = populationSize;

                await output.WriteLineAsync($"MPSO;function: ;{functionIndex};Population: ;{populationSize}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    MultiSwarmUniverse universe = new MultiSwarmUniverse(mpsoParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    Result result = new Simulation<Particle>().Run(
                        universe,
                        StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    mpsoMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, mpsoMetrics);
                await PrintAverageTime(output, times);
                times.Clear();

                await output.WriteLineAsync($"MSPSO;function: ;{functionIndex};Population: ;{populationSize}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    MultiSwarmUniverse universe = new MultiSwarmUniverse(mspsoParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    Result result = new Simulation<Particle>().Run(
                        universe,
                        StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    mspsoMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, mspsoMetrics);
                await PrintAverageTime(output, times);
                times.Clear();

                await output.WriteLineAsync($"BOA;function: ;{functionIndex};Population: ;{populationSize}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    ButterflyUniverse universe = new ButterflyUniverse(boaParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    Result result = new Simulation<Butterfly>().Run(
                        universe,
                        BoaExperiment.StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    boaMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, boaMetrics);
                await PrintAverageTime(output, times);
                times.Clear();
            }
            mpsoMetrics.Clear();
            mspsoMetrics.Clear();
            boaMetrics.Clear();

            for (int subPopulationCount = config.SwarmConfig.ExperimentParameters.Min.SubSwarmCount;
                subPopulationCount <= config.SwarmConfig.ExperimentParameters.Max.SubSwarmCount;
                subPopulationCount += config.SwarmConfig.ExperimentParameters.Rate.SubSwarmCount)
            {
                Logger.Print($"Subpopulations {subPopulationCount}");
                mpsoParams.SubSwarmCount = subPopulationCount;
                mspsoParams.SubSwarmCount = subPopulationCount;
                boaParams.SubPopulationCount = subPopulationCount;

                await output.WriteLineAsync($"MPSO;function: ;{functionIndex};Population: ;{subPopulationCount}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    MultiSwarmUniverse universe = new MultiSwarmUniverse(mpsoParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    Result result = new Simulation<Particle>().Run(
                        universe,
                        StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    mpsoMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, mpsoMetrics);
                await PrintAverageTime(output, times);
                times.Clear();

                await output.WriteLineAsync($"MSPSO;function: ;{functionIndex};Population: ;{subPopulationCount}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    MultiSwarmUniverse universe = new MultiSwarmUniverse(mspsoParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    Result result = new Simulation<Particle>().Run(
                        universe,
                        StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    mspsoMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, mspsoMetrics);
                await PrintAverageTime(output, times);
                times.Clear();

                await output.WriteLineAsync($"BOA;function: ;{functionIndex};Population: ;{subPopulationCount}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    ButterflyUniverse universe = new ButterflyUniverse(boaParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    Result result = new Simulation<Butterfly>().Run(
                        universe,
                        BoaExperiment.StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    boaMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, boaMetrics);
                await PrintAverageTime(output, times);
                times.Clear();
            }
            mpsoMetrics.Clear();
            mspsoMetrics.Clear();
            boaMetrics.Clear();

            // Boa
            boaParams = new BoaParameters(config.BoaConfig.BoaParameters);

            double original = boaParams.SensorModality;
            for (double sensorModality = config.BoaConfig.BoaExperimentParameters.Min.SensorModality;
                sensorModality <= config.BoaConfig.BoaExperimentParameters.Max.SensorModality;
                sensorModality += config.BoaConfig.BoaExperimentParameters.Rate.SensorModality)
            {
                boaParams.SensorModality = sensorModality;
                await output.WriteLineAsync($"BOA;function: ;{functionIndex};Sensor modality: ;{sensorModality}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    ButterflyUniverse universe = new ButterflyUniverse(boaParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    Result result = new Simulation<Butterfly>().Run(
                        universe,
                        BoaExperiment.StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    boaMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, boaMetrics);
                await PrintAverageTime(output, times);
                times.Clear();
            }
            boaParams.SensorModality = original;
            boaMetrics.Clear();

            original = boaParams.PowerExponent;
            for (double powerExponent = config.BoaConfig.BoaExperimentParameters.Min.PowerExponent;
                powerExponent <= config.BoaConfig.BoaExperimentParameters.Max.PowerExponent;
                powerExponent += config.BoaConfig.BoaExperimentParameters.Rate.PowerExponent)
            {
                boaParams.PowerExponent = powerExponent;
                await output.WriteLineAsync($"BOA;function: ;{functionIndex};Power exponent: ;{powerExponent}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    ButterflyUniverse universe = new ButterflyUniverse(boaParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    Result result = new Simulation<Butterfly>().Run(
                        universe,
                        BoaExperiment.StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    boaMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, boaMetrics);
                await PrintAverageTime(output, times);
                times.Clear();
            }
            boaParams.PowerExponent = original;
            boaMetrics.Clear();

            for (double switchProbability = config.BoaConfig.BoaExperimentParameters.Min.SwitchProbability;
                switchProbability <= config.BoaConfig.BoaExperimentParameters.Max.SwitchProbability;
                switchProbability += config.BoaConfig.BoaExperimentParameters.Rate.SwitchProbability)
            {
                boaParams.SwitchProbability = switchProbability;
                await output.WriteLineAsync($"BOA;function: ;{functionIndex};Switch probability: ;{switchProbability}; Experiments: ;{config.SwarmConfig.ExperimentsCount}");
                for (int i = 0; i < config.SwarmConfig.ExperimentsCount; ++i)
                {
                    ButterflyUniverse universe = new ButterflyUniverse(boaParams, function.UniverseSize, (function.Evaluate, function.Fitness));
                    Result result = new Simulation<Butterfly>().Run(
                        universe,
                        BoaExperiment.StopConditions[config.SwarmConfig.SwarmParameters.StopCondition]
                    );
                    boaMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, boaMetrics);
                await PrintAverageTime(output, times);
                times.Clear();
            }
            boaParams.PowerExponent = original;
        }

        private IEnumerable<double> MeanByIndex(IEnumerable<IList<double[]>> metric)
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
            await output.WriteLineAsync(string.Join(';', Enumerable.Range(1, 1 + metrics[0][MultiSwarmUniverse.BEST_ACCURACY_KEY].Count)));
            await output.WriteAsync("Mean accuracy;");
            await output.WriteLineAsync(string.Join(';', MeanByIndex(metrics.Select(d => d[MultiSwarmUniverse.BEST_ACCURACY_KEY]))));
            await output.WriteLineAsync();
            await output.WriteAsync("Iterations;");
            await output.WriteLineAsync(string.Join(';', Enumerable.Range(1, 1 + metrics[0][MultiSwarmUniverse.FITNESS_CHANGE_KEY].Count)));
            await output.WriteAsync("Absolute fitness change between iterations;");
            await output.WriteLineAsync(string.Join(';', MeanByIndex(metrics.Select(d => d[MultiSwarmUniverse.FITNESS_CHANGE_KEY]))));
            await output.WriteLineAsync();
        }

        private async Task PrintAverageTime(StreamWriter output, IList<long> times)
        {
            await output.WriteLineAsync();
            await output.WriteLineAsync($"Average time execution [ms]: {times.Average()}");
            await output.WriteLineAsync();
        }
    }
}
