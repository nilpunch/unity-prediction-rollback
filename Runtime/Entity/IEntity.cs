namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        bool IsAlive { get; }

        /// <summary>
        /// If true, then entity state in timeline is unstable. If entity does not change it to false in current step, then it will be safely removed from simulation on rollback.
        /// </summary>
        bool IsVolatile { get; }

        void Kill();
    }
}
