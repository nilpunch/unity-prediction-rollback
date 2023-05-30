using System;
using UnityEngine;

namespace UPR.Samples
{
    public class Lifetime : MonoBehaviour, IHistory, IRollback
    {
        [SerializeField] private bool _aliveInitially = true;

        private ChangeHistory<bool> _lifeHistory;

        public bool IsAlive
        {
            get
            {
                return _lifeHistory.Value;
            }
            set
            {
                _lifeHistory.Value = value;
                OnValueChanged();
            }
        }

        public void Init()
        {
            _lifeHistory = new ChangeHistory<bool>(_aliveInitially);
            OnValueChanged();
        }

        public void SaveStep()
        {
            _lifeHistory.SaveStep();
        }

        public void Rollback(int steps)
        {
            _lifeHistory.Rollback(steps);
            OnValueChanged();
        }

        public virtual void OnValueChanged() { }
    }
}
