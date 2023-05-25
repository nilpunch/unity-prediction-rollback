namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        EntityStatus CurrentStatus { get; }
        EntityStatus LastSavedStatus { get; }

        int LocalStep { get; }

        void Sleep();
        void Wake(int stepsAsleep);
    }
}
