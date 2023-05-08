namespace UPR
{
    public abstract class Entity : IEntity, ISimulation, IStateHistory
    {
        protected readonly Simulations LocalSimulations = new Simulations();
        protected readonly StateHistories LocalStateHistories = new StateHistories();

        protected Entity(EntityId id)
        {
            Id = id;
        }

        public EntityId Id { get; }

        public int HistoryLength => LocalStateHistories.HistoryLength;

        public void StepForward(long currentTick)
        {
            LocalSimulations.StepForward(currentTick);
        }

        public void SaveStep()
        {
            LocalStateHistories.SaveStep();
        }

        public void Rollback(int ticks)
        {
            LocalStateHistories.Rollback(ticks);
        }
    }
}
