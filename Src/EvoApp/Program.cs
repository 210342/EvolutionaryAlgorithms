using Evo.ParticleSwarm;
using Evo.ParticleSwarm.Experiment;
using Evo.Simulation;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Evo.EvoApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string filename = "SwarmInput.json";
            if (args.Length > 0)
            {
                filename = args[0];
            }
            SwarmConfig config = JsonSerializer.Deserialize<SwarmConfig>(File.ReadAllText(filename));
            if (config.SingleSimulation)
            {
                Function function = SwarmExperiment.Functions[config.FunctionIndex];
                using FileStream stream = new FileStream($"Simulation.csv", FileMode.Create);
                using StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("FunctionIndex;PopulationSize;MaxEpochs;StopCondition;MinAccuracy;ParticleChangeRate;ParticleChangeAddend;SwarmChangeRate;SwarmChangeAddend;DecelerationRate;InertiaWeight;InertiaWeightRate;InertiaWeightAddend;Time;Result;Error;ActualEpochs;ActualAccuracy");
                Stopwatch timer = new Stopwatch();
                timer.Start();
                SwarmUniverse universe = new SwarmUniverse(config.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                object result = new Simulation<Particle>().Run(
                    universe,
                    SwarmExperiment.StopConditions[config.SwarmParameters.StopCondition]
                );
                timer.Stop();
                double[] resultPosition = result as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await writer.WriteLineAsync($"{config.FunctionIndex};{config.SwarmParameters};{timer.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{universe.Epoch};{universe.Accuracy}");
            }
            else
            {
                for (int i = 0; i < SwarmExperiment.Functions.Length; ++i)
                {
                    using FileStream stream = new FileStream($"Results{i}.csv", FileMode.Create);
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine("FunctionIndex;PopulationSize;MaxEpochs;StopCondition;MinAccuracy;ParticleChangeRate;ParticleChangeAddend;SwarmChangeRate;SwarmChangeAddend;DecelerationRate;InertiaWeight;InertiaWeightRate;InertiaWeightAddend;Time;Result;Error;ActualEpochs;ActualAccuracy");
                    writer.WriteLine($"NULL;{config.SwarmParameters};NULL;NULL;NULL;NULL;NULL");
                    config = JsonSerializer.Deserialize<SwarmConfig>(File.ReadAllText(filename));
                    await new SwarmExperiment(new Logger()).Run(config, writer, (i, SwarmExperiment.Functions[i]));
                    config = JsonSerializer.Deserialize<SwarmConfig>(File.ReadAllText(filename));
                    config.SwarmParameters.StopCondition = "Accuracy";
                    await new SwarmExperiment(new Logger()).Run(config, writer, (i, SwarmExperiment.Functions[i]));
                }
            }
        }
    }
}
