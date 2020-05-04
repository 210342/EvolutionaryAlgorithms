using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using System;

namespace Evo.Simulation.GaSimulation
{
	public class GaPopulation : Population, Interfaces.IPopulation<GaOrganism>
	{
		public GaPopulation(int minSize, int maxSize, IChromosome adamChromosome) : base(minSize, maxSize, adamChromosome)
		{
		}

		public Interfaces.IUniverse<Interfaces.IPopulation<GaOrganism>, GaOrganism> Universe => throw new NotImplementedException();

		public uint Epoch { get; set; } = 0;

		public object Result => BestChromosome;

		public bool CanEvolve()
		{
			throw new NotImplementedException();
		}

		public void Evolve()
		{
			throw new NotImplementedException();
		}
	}
}
