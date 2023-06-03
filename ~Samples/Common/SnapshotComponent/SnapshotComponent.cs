using UnityEngine;

namespace UPR.Samples
{
    public abstract class SnapshotComponent<TData> : MonoBehaviour, IInitialize, IHistory, IRollback
    {
        private IValueHistory<TData> _memoryHistory;

        protected TData Data;

        protected abstract IValueHistory<TData> CreateHistory { get; }

        public void Initialize()
        {
            _memoryHistory = CreateHistory;
            Data = _memoryHistory.Value;
            OnDataChanged();
        }

        public void SaveStep()
        {
            _memoryHistory.Value = Data;
            _memoryHistory.SaveStep();
        }

        public void Rollback(int steps)
        {
            _memoryHistory.Rollback(steps);
            Data = _memoryHistory.Value;
            OnDataChanged();
        }

        protected virtual void OnDataChanged() {}
    }
}
