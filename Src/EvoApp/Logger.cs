using Evo.Simulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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
