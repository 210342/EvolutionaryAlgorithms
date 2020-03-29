using Evo.Simulation;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ParticleSwarm
{
    public class SwarmExperiment : Experiment<Particle, IPopulation<Particle>, SwarmConfig>
    {
        public override IEnumerable<(object, InputParameters)> Run(SwarmConfig config)
        {
            IList <(object, InputParameters)> results = new List<(object, InputParameters)>();
            for (int i = 0; i < Functions.Length; ++i)
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
                                    for (double velocityRate = config.SwarmParameters.VelocityRate;
                                    velocityRate <= config.ExperimentParameters.MaxVelocityRate;
                                    velocityRate += config.ExperimentParameters.VelocityRate)
                                    {
                                        SwarmParameters currentParams = new SwarmParameters()
                                        {
                                            UniverseSize = config.SwarmParameters.UniverseSize,
                                            StopCondition = config.SwarmParameters.StopCondition,
                                            PopulationSize = populationSize,
                                            MaxEpoch = maxEpoch,
                                            ParticleChangeRate = particleChangeRate,
                                            SwarmChangeRate = swarmChangeRate,
                                            DecelerationRate = decelerationRate,
                                            VelocityRate = velocityRate
                                        };
                                        object result = new Simulation<Particle>().Run(
                                            new SwarmUniverse(currentParams, Functions[i]),
                                            StopConditions[currentParams.StopCondition]
                                        );
                                        results.Add((result, currentParams));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return results;
        }
    }
}
