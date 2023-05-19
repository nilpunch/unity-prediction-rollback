namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        bool IsAlive { get; }

        int Age { get; }

        void Kill();
    }
}
