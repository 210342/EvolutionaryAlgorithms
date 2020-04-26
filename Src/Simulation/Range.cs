namespace Evo.Simulation
{
    public struct Range
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public Range(double min, double max)
        {
            Min = min;
            Max = max;
        }
    }
}
