using System.Collections.Generic;

namespace UPR
{
    public abstract class Entity : IEntity
    {
        protected readonly Simulations LocalSimulations = new Simulations();
        protected readonly StateHistories LocalStateHistories = new StateHistories();

        protected Entity(EntityId id)
        {
            Id = id;
        }

        public EntityId Id { get; }

        public void StepForward(long currentTick)
        {
            LocalSimulations.StepForward(currentTick);
        }

        public int HistoryLength => LocalStateHistories.HistoryLength;

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
