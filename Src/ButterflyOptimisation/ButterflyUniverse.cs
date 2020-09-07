using Evo.ButterflyOptimisation.Experiment;
using Evo.Simulation.Abstracts;
using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evo.ButterflyOptimisation
{
    public class ButterflyUniverse : Universe<Butterfly, IPopulation<Butterfly>>
    {
        public BoaParameters Parameters { get; }
        public Swarm Swarm { get; }
        public override IPopulation<Butterfly> Population => Swarm;
        public override double? Accuracy => null;

        public ButterflyUniverse(
            BoaParameters parameters,
            Simulation.Range[] size,
            (Func<double[], double>, Func<double, double, bool>) functions) 
            : base(size, 
                  parameters.MaxEpoch, 
                  parameters.MinAccuracy, 
                  functions)
        {
            Parameters = parameters;
            Swarm = new Swarm(this, parameters);
        }
        public ButterflyUniverse(
            BoaParameters parameters,
            Simulation.Range[] size,
            (Func<double[], double>, Func<double, double, bool>) functions,
            double[][] templateParticles)
            : base(size,
                  parameters.MaxEpoch,
                  parameters.MinAccuracy,
                  functions)
        {
            Parameters = parameters;
            Swarm = new Swarm(this, parameters, templateParticles);
        }

        public override void IterateEpoch()
        {
            base.IterateEpoch();
        }
    }
}
