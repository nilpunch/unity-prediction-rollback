namespace UPR
{
    public interface IEntity : IStateHistory, ISimulation
    {
        EntityId Id { get; }
    }
}
