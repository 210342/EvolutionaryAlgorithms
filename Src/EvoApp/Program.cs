using Evo.ParticleSwarm.Experiment;
using Evo.Simulation;
using Evo.Simulation.Interfaces;
using System;
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

            for (int i = 1; i < SwarmExperiment.Functions.Length; ++i)
            {
                SwarmConfig config = JsonSerializer.Deserialize<SwarmConfig>(File.ReadAllText(filename));
                using FileStream stream = new FileStream($"Results{i}.csv", FileMode.Create);
                using StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("FunctionIndex;PopulationSize;MaxEpochs;StopCondition;MinAccuracy;ParticleChangeRate;SwarmChangeRate;DecelerationRate;InertiaWeight;InertiaWeightRate;Time;Result;Error");
                writer.WriteLine($"NULL;{config.SwarmParameters};NULL;NULL;NULL");
                await new SwarmExperiment(new Logger()).Run(config, writer, (i, SwarmExperiment.Functions[i]));
            }
        }
    }
}
