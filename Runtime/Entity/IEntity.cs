namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        EntityId Id { get; }

        bool IsAlive { get; }

        /// <summary>
        /// Use whenever you want.
        /// </summary>
        void Kill();

        /// <summary>
        /// Use ONLY in rollback.
        /// </summary>
        void Resurrect();
    }
}
