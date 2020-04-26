using System;

namespace Evo.Simulation
{
    public class Function
    {
        public Func<double[], double> Evaluate { get; set; }
        public Func<double, double, bool> Fitness { get; set; }
        public Range[] UniverseSize { get; set; }
        public double ExpectedValue { get; set; }
        public double[] ExpectedPosition { get; set; }

        public Function(Range[] universeSize, Func<double[], double> evaluate, Func<double, double, bool> fitness, double expectedValue, double[] expectedPosition)
        {
            UniverseSize = universeSize;
            Evaluate = evaluate;
            Fitness = fitness;
            ExpectedValue = expectedValue;
            ExpectedPosition = expectedPosition;
        }
    }
}
