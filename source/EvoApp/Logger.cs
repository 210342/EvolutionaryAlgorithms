using Evo.Simulation.Interfaces;
using System;

namespace Evo.EvoApp
{
    public class Logger : ILogger
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
