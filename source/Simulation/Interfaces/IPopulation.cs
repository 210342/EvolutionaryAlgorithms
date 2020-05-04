namespace Evo.Simulation.Interfaces
{
    public interface IPopulation<OrganismType> 
        where OrganismType : IOrganism<OrganismType, IPopulation<OrganismType>>
    {
        IUniverse<IPopulation<OrganismType>, OrganismType> Universe { get; }
        uint Epoch { get; }
        object Result { get; }

        void Evolve();
        bool CanEvolve();
    }
}
