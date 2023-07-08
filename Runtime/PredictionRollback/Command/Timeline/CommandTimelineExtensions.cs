using System;
using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public static class CommandTimelineExtensions
    {
        public static ICommandTimeline<TCommand> AppendRepeatPrediction<TCommand>(this ICommandTimeline<TCommand> commandTimeline)
        {
            return new DecorateReadOnlyPart<TCommand>(commandTimeline, new RepeatPrediction<TCommand>(commandTimeline));
        }

        public static ICommandTimeline<TCommand> AppendFadeOutPrediction<TCommand>(this ICommandTimeline<TCommand> commandTimeline, int startDecayTick = 30, int decayDurationTicks = 60)
            where TCommand : IDecayingCommand<TCommand>
        {
            return new DecorateReadOnlyPart<TCommand>(commandTimeline, new FadeOutPrediction<TCommand>(commandTimeline, startDecayTick, decayDurationTicks));
        }

        public class DecorateReadOnlyPart<TCommand> : ICommandTimeline<TCommand>
        {
            private readonly ICommandTimeline<TCommand> _commandTimeline;
            private readonly IReadOnlyCommandTimeline<TCommand> _readOnlyCommandTimeline;

            public DecorateReadOnlyPart(ICommandTimeline<TCommand> commandTimeline, IReadOnlyCommandTimeline<TCommand> readOnlyCommandTimeline)
            {
                _commandTimeline = commandTimeline;
                _readOnlyCommandTimeline = readOnlyCommandTimeline;
            }

            public int GetLatestTickWithCommandBefore(int tickInclusive)
            {
                return _readOnlyCommandTimeline.GetLatestTickWithCommandBefore(tickInclusive);
            }

            public bool HasCommand(int tick)
            {
                return _readOnlyCommandTimeline.HasCommand(tick);
            }

            public bool HasExactCommand(int tick, TCommand command)
            {
                return _readOnlyCommandTimeline.HasExactCommand(tick, command);
            }

            public TCommand GetCommand(int tick)
            {
                return _readOnlyCommandTimeline.GetCommand(tick);
            }

            public IReadOnlyList<TCommand> FilledCommands => _commandTimeline.FilledCommands;

            public void RemoveAllCommandsInRange(int fromTickInclusive, int toTickInclusive)
            {
                _commandTimeline.RemoveAllCommandsInRange(fromTickInclusive, toTickInclusive);
            }

            public void RemoveAllCommandsDownTo(int tickExclusive)
            {
                _commandTimeline.RemoveAllCommandsDownTo(tickExclusive);
            }

            public void RemoveCommand(int tick)
            {
                _commandTimeline.RemoveCommand(tick);
            }

            public void InsertCommand(int tick, in TCommand command)
            {
                _commandTimeline.InsertCommand(tick, command);
            }
        }
    }
}
