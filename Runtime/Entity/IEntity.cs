namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        EntityStatus CurrentStatus { get; }
        bool HasUnsavedChanges { get; }

        int LocalStep { get; }

        void Sleep();
        void Wake(int stepsAsleep);
    }
}
