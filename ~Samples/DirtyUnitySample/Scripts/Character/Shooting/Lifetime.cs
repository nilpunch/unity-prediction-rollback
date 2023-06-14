using UnityEngine;
using UPR.PredictionRollback;
using UPR.Useful;

namespace UPR.Samples
{
    public class Lifetime : MonoBehaviour, IInitialize, IHistory, IRollback, IRebase
    {
        [SerializeField] private bool _aliveInitially = true;

        private RarelyChangingValue<bool> _isAlive;

        public bool IsAlive
        {
            get => _isAlive.Value;
            set
            {
                _isAlive.Value = value;
                OnAliveChanged();
            }
        }

        public void Initialize()
        {
            _isAlive = new RarelyChangingValue<bool>(_aliveInitially);
            OnAliveChanged();
        }

        public void SaveStep()
        {
            _isAlive.SaveStep();
        }

        public void Rollback(int steps)
        {
            _isAlive.Rollback(steps);
            OnAliveChanged();
        }

        public void ForgetFromBeginning(int steps)
        {
            _isAlive.ForgetFromBeginning(steps);
        }

        protected virtual void OnAliveChanged() {}
    }
}
