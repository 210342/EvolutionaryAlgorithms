using Evo.ButterflyOptimisation;
using Evo.ButterflyOptimisation.Experiment;
using Evo.Simulation;
using Evo.Simulation.GaSimulation;
using Evo.Simulation.Interfaces;
using Evo.Simulation.SimulationExperiment;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.ParticleSwarm.Experiment
{
    public class MixedExperiment : Experiment<Butterfly, IPopulation<Butterfly>, MixedConfig>
    {
        public MixedExperiment(ILogger logger) : base(logger)
        {
        }

        public async override Task Run(MixedConfig config, StreamWriter output, (int, Function) functionTuple)
        {
            Function function = functionTuple.Item2;
            int functionIndex = functionTuple.Item1;

            // Get template particles by using GA
            var minimums = function.UniverseSize.Select(r => r.Min).ToArray();
            var maximums = function.UniverseSize.Select(r => r.Max).ToArray();

            var chromosome = new FloatingPointChromosome(
                minimums,
                maximums,
                Enumerable.Repeat(64, minimums.Count()).ToArray(),
                Enumerable.Repeat(8, minimums.Count()).ToArray()
            );
            var gaPopulation = new GaPopulation((int)config.GaConfig.InputParameters.PopulationSize, (int)config.GaConfig.InputParameters.PopulationSize, chromosome);
            var ga = new GeneticAlgorithm(
                gaPopulation,
                new FuncFitness(c => -function.Evaluate((c as FloatingPointChromosome).ToFloatingPoints())),
                new TournamentSelection(4, true),
                new TwoPointCrossover(),
                new FlipBitMutation())
            {
                Termination = new GenerationNumberTermination((int)config.GaConfig.InputParameters.MaxEpoch)
            };
            double? accuracy = 0, previousBest = gaPopulation?.BestChromosome?.Fitness;
            ga.GenerationRan += (sender, e) =>
            {
                ++gaPopulation.Epoch;
                accuracy = gaPopulation?.BestChromosome?.Fitness - previousBest;
                previousBest = gaPopulation?.BestChromosome?.Fitness;
            };
            gaPopulation.MaxSize = (int)config.GaConfig.InputParameters.PopulationSize;
            gaPopulation.MinSize = (int)config.GaConfig.InputParameters.PopulationSize;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            ga.Start();
            timer.Stop();

            Result result = new Result()
            {
                ReturnedValue = gaPopulation.Result,
                Epochs = gaPopulation.Epoch,
                Accuracy = accuracy ?? 0,
                ElapsedMilliseconds = timer.ElapsedMilliseconds
            };

            double fitness = (result.ReturnedValue as FloatingPointChromosome)?.Fitness ?? 0;
            double[] position = (result.ReturnedValue as FloatingPointChromosome)?.ToFloatingPoints() ?? new double[] { 0.0 };
            double error = Math.Abs(function.ExpectedValue - fitness);
            await output.WriteLineAsync($"{functionIndex};{config.GaConfig.InputParameters.PopulationSize};{config.GaConfig.InputParameters.MaxEpoch};{result.ElapsedMilliseconds};[{string.Join(", ", position)}];{fitness};{error};{result.Epochs};{result.Accuracy}");

            double[][] templateParticles = gaPopulation
                .CurrentGeneration
                .Chromosomes
                .OfType<FloatingPointChromosome>()
                .Select(c => c.ToFloatingPoints())
                .ToArray();

            // Use template particles to setup BOA

            IList<long> times = new List<long>();
            var boaMetrics = new List<IDictionary<MetricsEnum, IList<double[]>>>();
            var boaParams = new BoaParameters(config.BoaConfig.BoaParameters);
            for (uint populationSize = config.BoaConfig.BoaExperimentParameters.Min.PopulationSize;
                populationSize <= config.BoaConfig.BoaExperimentParameters.Max.PopulationSize;
                populationSize += config.BoaConfig.BoaExperimentParameters.Rate.PopulationSize)
            {
                await output.WriteLineAsync($"BOA;function: ;{functionIndex};Population: ;{populationSize}; Epochs: ;{boaParams.MaxEpoch}; Experiments: ;{config.BoaConfig.ExperimentsCount}");
                for (int i = 0; i < config.BoaConfig.ExperimentsCount; ++i)
                {
                    boaParams.PopulationSize = populationSize;
                    ButterflyUniverse universe = new ButterflyUniverse(boaParams, function.UniverseSize, (function.Evaluate, function.Fitness), templateParticles);
                    result = new Simulation<Butterfly>().Run(
                        universe,
                        BoaExperiment.StopConditions[config.BoaConfig.BoaParameters.StopCondition]
                    );
                    boaMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, boaMetrics);
                await PrintAverageTime(output, times);
                boaMetrics.Clear();
                times.Clear();
            }
            boaParams = new BoaParameters(config.BoaConfig.BoaParameters);

            for (uint maxEpoch = config.BoaConfig.BoaExperimentParameters.Min.MaxEpoch;
                maxEpoch <= config.BoaConfig.BoaExperimentParameters.Max.MaxEpoch;
                maxEpoch += config.BoaConfig.BoaExperimentParameters.Rate.MaxEpoch)
            {
                boaParams.MaxEpoch = maxEpoch;
                await output.WriteLineAsync($"BOA;function: ;{functionIndex};Population: ;{boaParams.PopulationSize}; Epochs: ;{boaParams.MaxEpoch};Experiments: ;{config.BoaConfig.ExperimentsCount}");
                for (int i = 0; i < config.BoaConfig.ExperimentsCount; ++i)
                {
                    ButterflyUniverse universe = new ButterflyUniverse(boaParams, function.UniverseSize, (function.Evaluate, function.Fitness), templateParticles);
                    result = new Simulation<Butterfly>().Run(
                        universe,
                        BoaExperiment.StopConditions[config.BoaConfig.BoaParameters.StopCondition]
                    );
                    boaMetrics.Add(universe.Metrics);
                    times.Add(result.ElapsedMilliseconds);
                }
                await PrintMetrics(output, boaMetrics);
                await PrintAverageTime(output, times);
                boaMetrics.Clear();
                times.Clear();
            }
        }
    }
}
