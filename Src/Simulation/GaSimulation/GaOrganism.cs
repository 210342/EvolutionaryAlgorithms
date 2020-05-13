using Evo.Simulation.Interfaces;
using GeneticSharp.Domain.Chromosomes;
using System;
using System.Collections.Generic;

namespace Evo.Simulation.GaSimulation
{
    public class GaOrganism : ChromosomeBase, IOrganism<GaOrganism, IPopulation<GaOrganism>>
	{
		public GaOrganism(int length) : base(length)
		{

		}

		public double Result => Fitness ?? 0;

		public override IChromosome CreateNew()
		{
			throw new NotImplementedException();
		}

		public GaOrganism Evolve()
		{
			throw new NotImplementedException();
		}

		public GaOrganism Evolve(GaOrganism other)
		{
			throw new NotImplementedException();
		}

		public GaOrganism Evolve(IEnumerable<GaOrganism> others)
		{
			throw new NotImplementedException();
		}

		public GaOrganism Evolve(IPopulation<GaOrganism> population)
		{
			throw new NotImplementedException();
		}

		public override Gene GenerateGene(int geneIndex)
		{
			throw new NotImplementedException();
		}
	}
}
