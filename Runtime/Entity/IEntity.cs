namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        int LocalStep { get; }
    }
}
