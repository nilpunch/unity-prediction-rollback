using UnityEngine;

namespace UPR.Samples
{
    public abstract class UnityEntity : MonoBehaviour, IEntity
    {
        protected readonly Simulations LocalSimulations = new Simulations();
        protected readonly Rollbacks LocalRollbacks = new Rollbacks();
        protected readonly ReversibleHistories LocalReversibleHistories = new ReversibleHistories();

        public EntityId Id { get; set; }

        public int CurrentStep => LocalReversibleHistories.CurrentStep;

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
            LocalRollbacks.Rollback(steps);
        }
    }
}
