using Evo.Simulation;
using Evo.Simulation.GaSimulation;
using Evo.Simulation.SimulationExperiment;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Evo.ParticleSwarm.Experiment
{
    public class GaComparisonExperiment : Experiment<GaOrganism, GaPopulation, GaConfig>
    {
        private SwarmConfig _swarm;

        public GaComparisonExperiment(SwarmConfig swarm)
        {
            _swarm = swarm;
        }

        public override async Task Run(GaConfig config, StreamWriter output, (int, Function) functionTuple)
        {
            Function function = functionTuple.Item2;
            int functionIndex = functionTuple.Item1;
            var minimums = function.UniverseSize.Select(r => r.Min).ToArray();
            var maximums = function.UniverseSize.Select(r => r.Max).ToArray();

            var chromosome = new FloatingPointChromosome(
                minimums,
                maximums,
                Enumerable.Repeat(64, minimums.Count()).ToArray(),
                Enumerable.Repeat(8, minimums.Count()).ToArray()
            );
            var population = new GaPopulation((int)config.InputParameters.PopulationSize, (int)config.InputParameters.PopulationSize, chromosome);
            var ga = new GeneticAlgorithm(
                population,
                new FuncFitness(c => -function.Evaluate((c as FloatingPointChromosome).ToFloatingPoints())),
                new TournamentSelection(2, true),
                new ThreeParentCrossover(),
                new FlipBitMutation())
            {
                Termination = new GenerationNumberTermination((int)config.InputParameters.MaxEpoch)
            };
            double? accuracy = 0, previousBest = population?.BestChromosome?.Fitness;
            ga.GenerationRan += (sender, e) =>
            {
                ++population.Epoch;
                accuracy = population?.BestChromosome?.Fitness - previousBest;
                previousBest = population?.BestChromosome?.Fitness;
            };

            var populations = new List<(int, double)>();
            for (int populationSize = (int)config.ExperimentParameters.MinPopulation;
                populationSize <= (int)config.ExperimentParameters.MaxPopulation;
                populationSize += (int)config.ExperimentParameters.PopulationRate)
            {
                population.Epoch = 0;
                population.MaxSize = populationSize;
                population.MinSize = populationSize;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                ga.Start();
                timer.Stop();

                Result result = new Result()
                {
                    ReturnedValue = population.Result,
                    Epochs = population.Epoch,
                    Accuracy = accuracy ?? 0,
                    ElapsedMilliseconds = timer.ElapsedMilliseconds
                };

                double fitness = (result.ReturnedValue as FloatingPointChromosome)?.Fitness ?? 0;
                double[] position = (result.ReturnedValue as FloatingPointChromosome)?.ToFloatingPoints() ?? new double[] { 0.0 };
                double error = Math.Abs(function.ExpectedValue - fitness);
                await output.WriteLineAsync($"{functionIndex};{populationSize};{config.InputParameters.MaxEpoch};{result.ElapsedMilliseconds};[{string.Join(", ", position)}];{fitness};{error};{result.Epochs};{result.Accuracy}");
                populations.Add((populationSize, error));
            }
            config.InputParameters.PopulationSize = (uint)populations.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;
            population.MinSize = (int)config.InputParameters.PopulationSize;
            population.MaxSize = (int)config.InputParameters.PopulationSize;

            var swarmPopulations = new List<(int, double)>();
            for (int populationSize = (int)config.ExperimentParameters.MinPopulation;
                populationSize <= (int)config.ExperimentParameters.MaxPopulation;
                populationSize += (int)config.ExperimentParameters.PopulationRate)
            {
                _swarm.SwarmParameters.PopulationSize = (uint)populationSize;
                SwarmUniverse universe = new SwarmUniverse(_swarm.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                Result swarmResult = new Simulation<Particle>().Run(
                    universe,
                    SwarmExperiment.StopConditions[_swarm.SwarmParameters.StopCondition]
                );

                double[] resultPosition = swarmResult.ReturnedValue as double[];
                double resultValue = function.Evaluate(resultPosition);
                double swarmError = Math.Abs(function.ExpectedValue - resultValue);
                await output.WriteLineAsync($"{functionIndex};{populationSize};{_swarm.SwarmParameters.MaxEpoch};{swarmResult.ElapsedMilliseconds};[{string.Join(", ", resultPosition)}];{resultValue};{swarmError};{swarmResult.Epochs};{swarmResult.Accuracy}");
                swarmPopulations.Add((populationSize, swarmError));
            }
            _swarm.SwarmParameters.PopulationSize = (uint)swarmPopulations.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;
            

            var epochsList = new List<(int, double)>();
            for (int epochs = (int)config.ExperimentParameters.MinEpoch;
                epochs <= (int)config.ExperimentParameters.MaxEpoch;
                epochs += (int)config.ExperimentParameters.EpochRate)
            {
                population.Epoch = 0;
                ga.Termination = new GenerationNumberTermination(epochs);

                Stopwatch timer = new Stopwatch();
                timer.Start();
                ga.Start();
                timer.Stop();

                Result result = new Result()
                {
                    ReturnedValue = population.Result,
                    Epochs = population.Epoch,
                    Accuracy = accuracy ?? 0,
                    ElapsedMilliseconds = timer.ElapsedMilliseconds
                };

                double fitness = (result.ReturnedValue as FloatingPointChromosome)?.Fitness ?? 0;
                double[] position = (result.ReturnedValue as FloatingPointChromosome)?.ToFloatingPoints() ?? new double[] { 0.0 };
                double error = Math.Abs(function.ExpectedValue - fitness);
                await output.WriteLineAsync($"{functionIndex};{config.InputParameters.PopulationSize};{epochs};{result.ElapsedMilliseconds};[{string.Join(", ", position)}];{fitness};{error};{result.Epochs};{result.Accuracy}");
                epochsList.Add((epochs, error));
            }
            config.InputParameters.MaxEpoch = (uint)epochsList.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;

            var swarmEpochs = new List<(int, double)>();
            for (int epochs = (int)config.ExperimentParameters.MinEpoch;
                epochs <= (int)config.ExperimentParameters.MaxEpoch;
                epochs += (int)config.ExperimentParameters.EpochRate)
            {
                _swarm.SwarmParameters.MaxEpoch = (uint)epochs;
                SwarmUniverse universe = new SwarmUniverse(_swarm.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                Result swarmResult = new Simulation<Particle>().Run(
                    universe,
                    SwarmExperiment.StopConditions[_swarm.SwarmParameters.StopCondition]
                );

                double[] resultPosition = swarmResult.ReturnedValue as double[];
                double resultValue = function.Evaluate(resultPosition);
                double swarmError = Math.Abs(function.ExpectedValue - resultValue);
                await output.WriteLineAsync($"{functionIndex};{_swarm.SwarmParameters.PopulationSize};{epochs};{swarmResult.ElapsedMilliseconds};[{string.Join(", ", resultPosition)}];{resultValue};{swarmError};{swarmResult.Epochs};{swarmResult.Accuracy}");
                swarmEpochs.Add((epochs, swarmError));
            }
        }
    }
}
