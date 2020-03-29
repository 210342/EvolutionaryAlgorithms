using Evo.ParticleSwarm;
using Evo.Simulation;
using Evo.Simulation.Interfaces;
using System;
using System.IO;
using System.Text.Json;

namespace Evo.EvoApp
{
    class Program
    {
        static void Main(string[] args)
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
            /*
                        IUniverse<IPopulation<Particle>, Particle> universe = new SwarmUniverse(
                            config.SwarmParameters,
                            (x => (x[0] * x[0]) + (x[1] * x[1]),
                            (y1, y2) => y1 < y2)
                        );
                        object result = new Simulation<Particle>().Run(
                            universe,
                            universe => universe.Epoch >= config.SwarmParameters.MaxEpoch
                        );*/

            var results = new SwarmExperiment().Run(config);
        }
    }
}
