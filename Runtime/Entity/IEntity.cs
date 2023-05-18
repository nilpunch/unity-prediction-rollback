namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        EntityId Id { get; }

        bool IsAlive { get; }

        /// <summary>
        /// Unstable state. If entity does not change it to false in current step, then it will be safely removed from simulation.
        /// </summary>
        bool IsVolatile { get; }

        void Kill();
    }
}
