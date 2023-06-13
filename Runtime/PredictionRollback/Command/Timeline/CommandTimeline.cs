using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class CommandTimeline<TCommand> : ICommandTimeline<TCommand>, IReadOnlyCommandTimeline<TCommand>
    {
        private readonly Dictionary<int, TCommand> _timeline = new Dictionary<int, TCommand>();
        private readonly List<int> _filledTicksInOrder = new List<int>();

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

        public TCommand GetCommand(int tick)
        {
            return _timeline[tick];
        }

        public void RemoveAllCommandsDownTo(int tick)
        {
            for (int tickIndex = _filledTicksInOrder.Count - 1; tickIndex >= 0; tickIndex--)
            {
                int currentTick = _filledTicksInOrder[tickIndex];
                if (currentTick <= tick)
                    break;

                _timeline.Remove(currentTick);
                _filledTicksInOrder.RemoveAt(tickIndex);
            }
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
