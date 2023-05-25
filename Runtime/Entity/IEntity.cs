namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        EntityStatus Status { get; }

        int LocalStep { get; }

        void Sleep();
    }
}
