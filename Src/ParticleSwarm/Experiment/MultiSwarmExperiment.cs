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

            var mpsoMetrics = new List<IDictionary<MetricsEnum, IList<double[]>>>();
            var mspsoMetrics = new List<IDictionary<MetricsEnum, IList<double[]>>>();
            var boaMetrics = new List<IDictionary<MetricsEnum, IList<double[]>>>();
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
    }
}
