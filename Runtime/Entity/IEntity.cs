namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        EntityId Id { get; }
    }
}
