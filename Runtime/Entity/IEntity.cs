namespace UPR
{
    public interface IEntity : IHistory, ISimulation, IRollback
    {
        bool IsAlive { get; }

        /// <summary>
        /// Local existential age.<br/>
        /// Negative means not born yet and also not exists.
        /// Zero means just born, does not necessary correlate with existence.
        /// Positive means exists.
        /// </summary>
        int Step { get; }

        void Kill();
    }
}
