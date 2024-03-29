﻿using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class CommandTimeline<TCommand> : ICommandTimeline<TCommand>
    {
        private readonly IEqualityComparer<TCommand> _equalityComparer;
        private readonly Dictionary<int, TCommand> _timeline = new Dictionary<int, TCommand>();
        private readonly List<int> _filledTicksInOrder = new List<int>();

        public IReadOnlyList<TCommand> FilledCommands => new ReadOnlyCommandList<TCommand>(_timeline, _filledTicksInOrder);

        public CommandTimeline(IEqualityComparer<TCommand> equalityComparer = null)
        {
            _equalityComparer = equalityComparer ?? EqualityComparer<TCommand>.Default;
        }

        public int GetLatestTickWithCommandBefore(int tickInclusive)
        {
            int tickIndex = _filledTicksInOrder.BinarySearch(tickInclusive);

            if (tickIndex < 0)
            {
                tickIndex = ~tickIndex - 1;

                if (tickIndex < 0)
                {
                    return -1;
                }
            }

            return _filledTicksInOrder[tickIndex];
        }

        public bool HasCommand(int tick)
        {
            return _timeline.ContainsKey(tick);
        }

        public bool HasExactCommand(int tick, TCommand command)
        {
            if (_timeline.TryGetValue(tick, out var existedCommand))
            {
                return _equalityComparer.Equals(existedCommand, command);
            }

            return false;
        }

        public TCommand GetCommand(int tick)
        {
            return _timeline[tick];
        }

        public void RemoveAllCommandsDownTo(int tickExclusive)
        {
            for (int tickIndex = _filledTicksInOrder.Count - 1; tickIndex >= 0; tickIndex--)
            {
                int currentTick = _filledTicksInOrder[tickIndex];
                if (currentTick <= tickExclusive)
                    break;

                _timeline.Remove(currentTick);
                _filledTicksInOrder.RemoveAt(tickIndex);
            }
        }

        public void RemoveAllCommandsInRange(int fromTickInclusive, int toTickInclusive)
        {
            int earliestTickIndex = _filledTicksInOrder.BinarySearch(fromTickInclusive);
            if (earliestTickIndex < 0)
            {
                earliestTickIndex = ~earliestTickIndex - 1;

                if (earliestTickIndex < 0)
                {
                    return;
                }
            }

            int latestTickIndex = _filledTicksInOrder.BinarySearch(toTickInclusive);
            if (latestTickIndex < 0)
            {
                latestTickIndex = ~latestTickIndex;
            }

            for (int tickIndex = earliestTickIndex; tickIndex <= latestTickIndex; tickIndex++)
            {
                _timeline.Remove(_filledTicksInOrder[tickIndex]);
            }

            _filledTicksInOrder.RemoveRange(earliestTickIndex, latestTickIndex - earliestTickIndex);
        }

        public void RemoveCommand(int tick)
        {
            if (_timeline.ContainsKey(tick))
            {
                _timeline.Remove(tick);
                _filledTicksInOrder.RemoveAt(_filledTicksInOrder.BinarySearch(tick));
            }
        }

        public void InsertCommand(int tick, in TCommand command)
        {
            if (!_timeline.ContainsKey(tick))
            {
                int tickIndex = _filledTicksInOrder.BinarySearch(tick);
                _filledTicksInOrder.Insert(~tickIndex, tick);
                _timeline.Add(tick, command);
            }
            else
            {
                _timeline[tick] = command;
            }
        }
    }
}
