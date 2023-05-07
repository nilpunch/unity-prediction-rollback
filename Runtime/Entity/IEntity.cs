namespace UPR
{
    public interface IEntity : ISimulation, IStateHistory
    {
        EntityId Id { get; }
    }
}
