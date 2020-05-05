using Evo.ParticleSwarm;
using Evo.ParticleSwarm.Experiment;
using Evo.Simulation;
using Evo.Simulation.GaSimulation;
using Evo.Simulation.SimulationExperiment;
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
            await MPSO(args);
        }

        public static async Task MPSO(string[] args)
        {
            string swarmConfigFilename = "SwarmInput.json";
            if (args.Length > 0)
            {
                swarmConfigFilename = args[0];
            }
            SwarmConfig swarmConfig = JsonSerializer.Deserialize<SwarmConfig>(File.ReadAllText(swarmConfigFilename));
        }

        public static async Task PSO(string[] args)
        {
            string swarmConfigFilename = "SwarmInput.json";
            string gaConfigFilename = "GaInput.json";
            if (args.Length > 0)
            {
                swarmConfigFilename = args[0];
            }
            SwarmConfig swarmConfig = JsonSerializer.Deserialize<SwarmConfig>(File.ReadAllText(swarmConfigFilename));
            if (File.Exists(gaConfigFilename))
            {
                GaConfig gaConfig = JsonSerializer.Deserialize<GaConfig>(File.ReadAllText(gaConfigFilename));
                if (gaConfig != null)
                {
                    Function function = GaComparisonExperiment.Functions[gaConfig.FunctionIndex];
                    using FileStream stream = new FileStream($"Simulation_function{gaConfig.FunctionIndex}.csv", FileMode.Create);
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine("FunctionIndex;PopulationSize;MaxEpochs;Time;Result;ResultFitness;Error;ActualEpochs;ActualAccuracy");
                    var experiment = new GaComparisonExperiment(swarmConfig);
                    await experiment.Run(gaConfig, writer, (gaConfig.FunctionIndex, function));
                }
            }
            else if (swarmConfig.SingleSimulation)
            {
                Function function = SwarmExperiment.Functions[swarmConfig.FunctionIndex];
                using FileStream stream = new FileStream($"Simulation.csv", FileMode.Create);
                using StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("FunctionIndex;PopulationSize;MaxEpochs;StopCondition;MinAccuracy;ParticleChangeRate;ParticleChangeAddend;SwarmChangeRate;SwarmChangeAddend;DecelerationRate;InertiaWeight;InertiaWeightRate;InertiaWeightAddend;Time;Result;Error;ActualEpochs;ActualAccuracy");
                SwarmUniverse universe = new SwarmUniverse(swarmConfig.SwarmParameters, function.UniverseSize, (function.Evaluate, function.Fitness));
                Result result = new Simulation<Particle>().Run(
                    universe,
                    SwarmExperiment.StopConditions[swarmConfig.SwarmParameters.StopCondition]
                );
                double[] resultPosition = result.ReturnedValue as double[];
                double error = Math.Abs(function.ExpectedValue - function.Evaluate(resultPosition));
                await writer.WriteLineAsync($"{swarmConfig.FunctionIndex};{swarmConfig.SwarmParameters};{result.ElapsedMilliseconds};[{string.Join(",", resultPosition)}];{error};{result.Epochs};{result.Accuracy}");
            }
            else
            {
                for (int j = 0; j < swarmConfig.ExperimentsCount; ++j)
                {
                    for (int i = 0; i < SwarmExperiment.Functions.Length; ++i)
                    {
                        using FileStream stream = new FileStream($"Results{i}_{j}.csv", FileMode.Create);
                        using StreamWriter writer = new StreamWriter(stream);
                        writer.WriteLine("FunctionIndex;PopulationSize;MaxEpochs;StopCondition;MinAccuracy;ParticleChangeRate;ParticleChangeAddend;SwarmChangeRate;SwarmChangeAddend;DecelerationRate;InertiaWeight;InertiaWeightRate;InertiaWeightAddend;Time;Result;Error;ActualEpochs;ActualAccuracy");
                        writer.WriteLine($"NULL;{swarmConfig.SwarmParameters};NULL;NULL;NULL;NULL;NULL");
                        swarmConfig = JsonSerializer.Deserialize<SwarmConfig>(File.ReadAllText(swarmConfigFilename));
                        await new SwarmExperiment(new Logger()).Run(swarmConfig, writer, (i, SwarmExperiment.Functions[i]));
                        swarmConfig = JsonSerializer.Deserialize<SwarmConfig>(File.ReadAllText(swarmConfigFilename));
                        swarmConfig.SwarmParameters.StopCondition = "Accuracy";
                        await new SwarmExperiment(new Logger()).Run(swarmConfig, writer, (i, SwarmExperiment.Functions[i]));
                    }
                }
            }

        }
    }
}
