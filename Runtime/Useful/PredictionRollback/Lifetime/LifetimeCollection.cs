using System;
using System.Collections.Generic;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class LifetimeCollection<TEntry> : IHistory, IRollback, IRebase
    {
        private readonly Dictionary<TEntry, RarelyChangingValue<LiveStatus>> _lifetimes = new Dictionary<TEntry, RarelyChangingValue<LiveStatus>>();
        private readonly IPool<RarelyChangingValue<LiveStatus>> _pool = new Pool<RarelyChangingValue<LiveStatus>>(new LiveStatusFactory());

        private readonly Dictionary<TEntry, LiveStatus> _liveChanges = new Dictionary<TEntry, LiveStatus>();

        private int OriginTick { get; set; }
        private int CurrentTick { get; set; }

        public void MarkAlive(TEntry entry)
        {
            if (_liveChanges.ContainsKey(entry))
            {
                _liveChanges[entry] = LiveStatus.Alive;
            }
            else
            {
                _liveChanges.Add(entry, LiveStatus.Alive);
            }
        }

        public void MarkDead(TEntry entry)
        {
            if (_liveChanges.ContainsKey(entry))
            {
                _liveChanges[entry] = LiveStatus.Dead;
            }
            else
            {
                _liveChanges.Add(entry, LiveStatus.Dead);
            }
        }

        public bool IsAlive(TEntry entry)
        {
            if (_liveChanges.TryGetValue(entry, out var liveStatus))
            {
                return liveStatus == LiveStatus.Alive;
            }

            if (_lifetimes.TryGetValue(entry, out var lifetime))
            {
                return lifetime.Value == LiveStatus.Alive;
            }

            return false;
        }

        public void SaveStep()
        {
            CommitChanges();

            foreach (var liveStatus in _lifetimes.Values)
            {
                liveStatus.SaveStep();
            }

            CurrentTick += 1;

            void CommitChanges()
            {
                foreach (var liveChange in _liveChanges)
                {
                    if (_lifetimes.TryGetValue(liveChange.Key, out var lifetime))
                    {
                        lifetime.Value = liveChange.Value;
                    }
                    else if (liveChange.Value == LiveStatus.Alive)
                    {
                        _lifetimes.Add(liveChange.Key, _pool.Get());
                    }
                }

                _liveChanges.Clear();
            }
        }

        private readonly List<TEntry> _toDeleteOnRollback = new List<TEntry>();

        public void Rollback(int steps)
        {
            RemoveIrrelevant();

            foreach (var liveStatus in _lifetimes.Values)
            {
                liveStatus.Rollback(steps);
            }

            CurrentTick -= steps;

            void RemoveIrrelevant()
            {
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
            }
        }

        public void ForgetFromBeginning(int steps)
        {
            OriginTick += steps;

            foreach (var liveStatus in _lifetimes.Values)
            {
                int entityHistoryBegin = CurrentTick - liveStatus.StepsSaved;
                int canForgetSteps = Math.Max(OriginTick - entityHistoryBegin, 0);
                liveStatus.ForgetFromBeginning(Math.Min(canForgetSteps, steps));
            }
        }

        private enum LiveStatus { Alive, Dead }

        private class LiveStatusFactory : IFactory<RarelyChangingValue<LiveStatus>>
        {
            public RarelyChangingValue<LiveStatus> Create()
            {
                return new RarelyChangingValue<LiveStatus>();
            }
        }
    }
}
