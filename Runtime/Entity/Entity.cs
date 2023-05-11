namespace UPR
{
    public abstract class Entity : IEntity
    {
        protected readonly Simulations LocalSimulations = new Simulations();
        protected readonly Histories LocalHistories = new Histories();
        protected readonly Rollbacks LocalRollbacks = new Rollbacks();

        protected Entity(EntityId id)
        {
            Id = id;
        }

        public EntityId Id { get; }

        public int CurrentStep => LocalHistories.CurrentStep;

        public void StepForward()
        {
            LocalSimulations.StepForward();
        }

        public void SaveStep()
        {
            LocalHistories.SaveStep();
        }

        public void Rollback(int steps)
        {
            LocalRollbacks.Rollback(steps);
        }
    }
}
