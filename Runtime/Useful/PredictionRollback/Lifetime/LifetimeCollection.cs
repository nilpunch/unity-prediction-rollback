using System;
using System.Collections.Generic;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class LifetimeCollection<TEntry> : IHistory, IRollback, IRebase
    {
        private readonly Dictionary<TEntry, RarelyChangingValue<LiveStatus>> _lifetimes = new Dictionary<TEntry, RarelyChangingValue<LiveStatus>>();
        private readonly IPool<RarelyChangingValue<LiveStatus>> _pool = new Pool<RarelyChangingValue<LiveStatus>>(new LiveStatusFactory());

        public void MarkAlive(TEntry entry)
        {
            if (_lifetimes.TryGetValue(entry, out var lifetime))
            {
                lifetime.Value = LiveStatus.Alive;
                return;
            }
            _lifetimes.Add(entry, _pool.Get());
        }

        public void MarkDead(TEntry entry)
        {
            if (_lifetimes.TryGetValue(entry, out var lifetime))
            {
                lifetime.Value = LiveStatus.Dead;
            }
        }

        public void SaveStep()
        {
            foreach (var liveStatus in _lifetimes.Values)
            {
                liveStatus.SaveStep();
            }
        }

        private readonly List<TEntry> _toDeleteOnRollback = new List<TEntry>();

        public void Rollback(int steps)
        {
            // Remove irrelevant lifetimes
            foreach (var entryLifetime in _lifetimes)
            {
                if (entryLifetime.Value.StepsSaved < steps)
                {
                    _toDeleteOnRollback.Add(entryLifetime.Key);
                    _pool.Return(entryLifetime.Value);
                }
            }
            foreach (var entry in _toDeleteOnRollback)
            {
                _lifetimes.Remove(entry);
            }
            _toDeleteOnRollback.Clear();

            // Actual rollback
            foreach (var liveStatus in _lifetimes.Values)
            {
                liveStatus.Rollback(steps);
            }
        }

        public void ForgetFromBeginning(int steps)
        {
            throw new System.NotImplementedException();
        }

        private enum LiveStatus { Alive, Dead }

        private class LiveStatusFactory : IFactory<RarelyChangingValue<LiveStatus>>
        {
            public RarelyChangingValue<LiveStatus> Create()
            {
                return new RarelyChangingValue<LiveStatus>(LiveStatus.Alive);
            }
        }
    }
}
