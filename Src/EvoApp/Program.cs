using Evo.ParticleSwarm;
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

            SwarmConfig config = JsonSerializer.Deserialize<SwarmConfig>(File.ReadAllText(filename));

            if (config is null)
            {
                return;
            }


            using (FileStream stream = new FileStream("Results.csv", FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("FunctionIndex;PopulationSize;MaxEpochs;ParticleChangeRate;SwarmChangeRate;DecelerationRate;InertiaWeight;InertiaWeightRate;Time;StopCondition;Result;Error");
                    await new SwarmExperiment().Run(config, writer);
                }
            }
        }
    }
}
