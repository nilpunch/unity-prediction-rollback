using UnityEngine;

namespace UPR.Samples
{
    public abstract class UnityEntity : MonoBehaviour, IEntity
    {
        protected readonly Simulations LocalSimulations = new Simulations();
        protected readonly ReversibleHistories LocalReversibleHistories = new ReversibleHistories();

        public EntityId Id { get; set; }

        public bool IsAlive { get; private set; } = true;

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
