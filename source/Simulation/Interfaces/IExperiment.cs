using System.IO;
using System.Threading.Tasks;

namespace Evo.Simulation.Interfaces
{
    public interface IExperiment<ParametersType>
    {
        Task Run(ParametersType parameters, StreamWriter output, (int, Function) function);
    }
}
