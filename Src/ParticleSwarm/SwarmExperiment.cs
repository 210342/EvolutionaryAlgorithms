using Evo.Simulation;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Evo.ParticleSwarm
{
    public class SwarmExperiment : Experiment<Particle, IPopulation<Particle>, SwarmConfig>
    {
        public override async Task Run(SwarmConfig config, StreamWriter output)
        {
            for (int functionIndex = 0; functionIndex < Functions.Length; ++functionIndex)
            {
                for (uint populationSize = config.SwarmParameters.PopulationSize; 
                    populationSize <= config.ExperimentParameters.MaxPopulation; 
                    populationSize += config.ExperimentParameters.PopulationRate)
                {
                    for (uint maxEpoch = config.SwarmParameters.MaxEpoch;
                        maxEpoch <= config.ExperimentParameters.MaxEpoch;
                        maxEpoch += config.ExperimentParameters.EpochRate)
                    {
                        for (double particleChangeRate = config.SwarmParameters.ParticleChangeRate;
                            particleChangeRate <= config.ExperimentParameters.MaxParticleChangeRate;
                            particleChangeRate += config.ExperimentParameters.ParticleChangeRate)
                        {
                            for (double swarmChangeRate = config.SwarmParameters.SwarmChangeRate;
                            swarmChangeRate <= config.ExperimentParameters.MaxSwarmChangeRate;
                            swarmChangeRate += config.ExperimentParameters.SwarmChangeRate)
                            {
                                for (double decelerationRate = config.SwarmParameters.DecelerationRate;
                                decelerationRate <= config.ExperimentParameters.MaxDecelerationRate;
                                decelerationRate += config.ExperimentParameters.DecelerationRate)
                                {
                                    for (double inertiaWeight = config.SwarmParameters.InertiaWeight;
                                    inertiaWeight <= config.ExperimentParameters.MaxInertiaWeight;
                                    inertiaWeight += config.ExperimentParameters.InertiaWeight)
                                    {
                                        for (double inertiaWeightRate = config.SwarmParameters.InertiaWeightRate;
                                        inertiaWeightRate <= config.ExperimentParameters.MaxInertiaWeightRate;
                                        inertiaWeightRate += config.ExperimentParameters.InertiaWeightRate)
                                        {
                                            Function function = Functions[functionIndex];
                                            SwarmParameters currentParams = new SwarmParameters()
                                            {
                                                StopCondition = config.SwarmParameters.StopCondition,
                                                PopulationSize = populationSize,
                                                MaxEpoch = maxEpoch,
                                                ParticleChangeRate = particleChangeRate,
                                                SwarmChangeRate = swarmChangeRate,
                                                DecelerationRate = decelerationRate,
                                                InertiaWeight = inertiaWeight,
                                                InertiaWeightRate = config.SwarmParameters.InertiaWeightRate
                                            };
                                            await Task.Run(async () => {
                                                Stopwatch timer = new Stopwatch();
                                                timer.Start();
                                                object result = new Simulation<Particle>().Run(
                                                    new SwarmUniverse(currentParams, function.UniverseSize, (function.Evaluate, function.Fitness)),
                                                    StopConditions[currentParams.StopCondition]
                                                );
                                                timer.Stop();
                                                double[] resultPosition = result as double[];
                                                await output.WriteLineAsync($"{functionIndex};{populationSize};{maxEpoch};{particleChangeRate};{swarmChangeRate};{decelerationRate};{inertiaWeight};{inertiaWeightRate};{timer.ElapsedMilliseconds};{config.SwarmParameters.StopCondition};[{string.Join(",", resultPosition)}];{function.ExpectedValue - function.Evaluate(resultPosition)}");
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
