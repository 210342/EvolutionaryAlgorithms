using Evo.Simulation;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.ParticleSwarm.Experiment
{
    public class SwarmExperiment : Experiment<Particle, IPopulation<Particle>, SwarmConfig>
    {
        public ILogger Logger { get; set; }

        public SwarmExperiment(ILogger logger)
        {
            Logger = logger;
        }

        public override async Task Run(SwarmConfig config, StreamWriter output, (int, Function) functionTuple)
        {
            Function function = functionTuple.Item2;
            int functionIndex = functionTuple.Item1;
            Logger.Print($"Function {functionIndex}");
            var populations = new List<(uint, double)>();
            
            for (uint populationSize = config.ExperimentParameters.Min.PopulationSize;
                populationSize <= config.ExperimentParameters.Max.PopulationSize;
                populationSize += config.ExperimentParameters.Rate.PopulationSize)
            {
                config.SwarmParameters.PopulationSize = populationSize;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();

                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
                populations.Add((populationSize, error));
                Logger.Print($"Population {populationSize}");
            }
            config.SwarmParameters.PopulationSize = populations.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;

            var epochs = new List<(uint, double)>();
            for (uint maxEpoch = config.ExperimentParameters.Min.MaxEpoch;
                maxEpoch <= config.ExperimentParameters.Max.MaxEpoch;
                maxEpoch += config.ExperimentParameters.Rate.MaxEpoch)
            {
                config.SwarmParameters.MaxEpoch = maxEpoch;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();

                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
                epochs.Add((maxEpoch, error));
                Logger.Print($"Epochs {maxEpoch}");
            }
            config.SwarmParameters.MaxEpoch = epochs.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;

            var particleChangeRates = new List<(double, double)>();
            for (double particleChangeRate = config.ExperimentParameters.Min.ParticleChangeRate;
                particleChangeRate <= config.ExperimentParameters.Max.ParticleChangeRate;
                particleChangeRate += config.ExperimentParameters.Rate.ParticleChangeRate)
            {
                config.SwarmParameters.ParticleChangeRate = particleChangeRate;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();

                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
                particleChangeRates.Add((particleChangeRate, error));
                Logger.Print($"Particle Change Rate {particleChangeRate}");
            }
            config.SwarmParameters.ParticleChangeRate = particleChangeRates.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;

            var particleChangeAddends = new List<(double, double)>();
            for (double particleChangeAddend = config.ExperimentParameters.Min.ParticleChangeAddend;
                particleChangeAddend <= config.ExperimentParameters.Max.ParticleChangeAddend;
                particleChangeAddend += config.ExperimentParameters.Rate.ParticleChangeAddend)
            {
                config.SwarmParameters.ParticleChangeAddend = particleChangeAddend;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();

                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
                particleChangeAddends.Add((particleChangeAddend, error));
                Logger.Print($"Particle Change Addend {particleChangeAddend}");
            }
            config.SwarmParameters.ParticleChangeAddend = particleChangeAddends.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;

            var swarmChangeRates = new List<(double, double)>();
            for (double swarmChangeRate = config.ExperimentParameters.Min.SwarmChangeRate;
                swarmChangeRate <= config.ExperimentParameters.Max.SwarmChangeRate;
                swarmChangeRate += config.ExperimentParameters.Rate.SwarmChangeRate)
            {
                config.SwarmParameters.SwarmChangeRate = swarmChangeRate;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();

                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
                swarmChangeRates.Add((swarmChangeRate, error));
                Logger.Print($"Swarm Change Rate {swarmChangeRate}");
            }
            config.SwarmParameters.SwarmChangeRate = swarmChangeRates.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;

            var swarmChangeAddends = new List<(double, double)>();
            for (double swarmChangeAddend = config.ExperimentParameters.Min.SwarmChangeAddend;
                swarmChangeAddend <= config.ExperimentParameters.Max.SwarmChangeAddend;
                swarmChangeAddend += config.ExperimentParameters.Rate.SwarmChangeAddend)
            {
                config.SwarmParameters.SwarmChangeAddend = swarmChangeAddend;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();

                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
                swarmChangeAddends.Add((swarmChangeAddend, error));
                Logger.Print($"Swarm Change Addend {swarmChangeAddend}");
            }
            config.SwarmParameters.SwarmChangeAddend = swarmChangeAddends.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;

            var decelerationRates = new List<(double, double)>();
            for (double decelerationRate = config.ExperimentParameters.Min.DecelerationRate;
                decelerationRate <= config.ExperimentParameters.Max.DecelerationRate;
                decelerationRate += config.ExperimentParameters.Rate.DecelerationRate)
            {
                config.SwarmParameters.DecelerationRate = decelerationRate;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();

                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
                decelerationRates.Add((decelerationRate, error));
                Logger.Print($"Deceleration Rate {decelerationRate}");
            }
            config.SwarmParameters.DecelerationRate = decelerationRates.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;

            var inertiaWeights = new List<(double, double)>();
            for (double inertiaWeight = config.ExperimentParameters.Min.InertiaWeight;
                inertiaWeight <= config.ExperimentParameters.Max.InertiaWeight;
                inertiaWeight += config.ExperimentParameters.Rate.InertiaWeight)
            {
                config.SwarmParameters.InertiaWeight = inertiaWeight;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();

                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
                inertiaWeights.Add((inertiaWeight, error));
                Logger.Print($"Inertia Weight {inertiaWeight}");
            }
            config.SwarmParameters.InertiaWeight = inertiaWeights.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;

            var inertiaWeightRates = new List<(double, double)>();
            for (double inertiaWeightRate = config.ExperimentParameters.Min.InertiaWeightRate;
                inertiaWeightRate <= config.ExperimentParameters.Max.InertiaWeightRate;
                inertiaWeightRate += config.ExperimentParameters.Rate.InertiaWeightRate)
            {
                config.SwarmParameters.InertiaWeightRate = inertiaWeightRate;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();

                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
                inertiaWeightRates.Add((inertiaWeightRate, error));
                Logger.Print($"Inertia Weight Rate {inertiaWeightRate}");
            }
            config.SwarmParameters.InertiaWeightRate = inertiaWeightRates.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;

            var inertiaWeightAddends = new List<(double, double)>();
            for (double inertiaWeightAddend = config.ExperimentParameters.Min.InertiaWeightAddend;
                inertiaWeightAddend <= config.ExperimentParameters.Max.InertiaWeightAddend;
                inertiaWeightAddend += config.ExperimentParameters.Rate.InertiaWeightAddend)
            {
                config.SwarmParameters.InertiaWeightAddend = inertiaWeightAddend;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();

                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
                inertiaWeightAddends.Add((inertiaWeightAddend, error));
                Logger.Print($"Inertia Weight Addend {inertiaWeightAddend}");
            }
            config.SwarmParameters.InertiaWeightAddend = inertiaWeightAddends.Aggregate((p1, p2) => p1.Item2 < p2.Item2 ? p1 : p2).Item1;
            await output.WriteLineAsync($"{functionIndex};{config.SwarmParameters};NULL;NULL;NULL;NULL;NULL");
        }
    }
}
