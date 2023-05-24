namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        EntityStatus Status { get; }

        int LocalStep { get; }
        int GlobalStep { get; }

        void Sleep();
    }
}
