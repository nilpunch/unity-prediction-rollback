namespace UPR
{
    public abstract class Entity : IEntity
    {
        protected readonly Simulations LocalSimulations = new Simulations();
        protected readonly ReversibleHistories LocalReversibleHistories = new ReversibleHistories();

        protected Entity(EntityId id)
        {
            Id = id;
            IsAlive = true;
        }

        public EntityId Id { get; }

        public bool IsAlive { get; private set; }

        public int CurrentStep => LocalReversibleHistories.CurrentStep;

        public void Kill()
        {
            IsAlive = false;
        }

        public void Resurrect()
        {
            IsAlive = true;
        }

        public void StepForward()
        {
            LocalSimulations.StepForward();
        }

        public void SaveStep()
        {
            LocalReversibleHistories.SaveStep();
        }

        public void Rollback(int steps)
        {
            LocalReversibleHistories.Rollback(steps);
        }
    }
}
