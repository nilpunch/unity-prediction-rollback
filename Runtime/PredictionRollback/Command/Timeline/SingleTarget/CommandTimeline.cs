using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class CommandTimeline<TCommand> : ICommandTimeline<TCommand>
    {
        private readonly ICommandRouter<TCommand> _commandRouter;
        private readonly TargetId _targetId;

        private readonly Dictionary<int, TCommand> _timeline = new Dictionary<int, TCommand>();
        private readonly List<int> _filledTicksInOrder = new List<int>();

        public CommandTimeline(ICommandRouter<TCommand> commandRouter, TargetId targetId)
        {
            _commandRouter = commandRouter;
            _targetId = targetId;
        }

        public int GetLatestTickWithCommand(int tick)
        {
            int tickIndex = _filledTicksInOrder.BinarySearch(tick);

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

        public void ExecuteCommand(int tick)
        {
            if (_timeline.TryGetValue(tick, out var command))
            {
                _commandRouter.ForwardCommand(command, _targetId);
            }
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
