using Evo.Simulation.Interfaces;
using System;
using System.Diagnostics;

namespace Evo.Simulation.SimulationExperiment
{
    public class Simulation<OrganismType> : ISimulation<OrganismType>
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        public Result Run(
            IUniverse<IPopulation<OrganismType>, OrganismType> universe,
            Func<IUniverse<IPopulation<OrganismType>, OrganismType>, bool> stopCondition)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            do
            {
                universe.Population.Evolve();
            }
            while (!stopCondition(universe));
            timer.Stop();
            return new Result()
            {
                ReturnedValue = universe.Population.Result,
                Epochs = universe.Population.Epoch,
                Accuracy = universe.Accuracy,
                ElapsedMilliseconds = timer.ElapsedMilliseconds
            };
        }
    }
}
