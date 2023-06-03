using System;
using UnityEngine;

namespace UPR.Samples
{
    public class Lifetime : MonoBehaviour, IHistory, IRollback, IInitialize
    {
        [SerializeField] private bool _aliveInitially = true;

        private ReversibleValue<bool> _isAlive;

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
            _isAlive = new ReversibleValue<bool>(_aliveInitially);
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

        protected virtual void OnAliveChanged() {}
    }
}
