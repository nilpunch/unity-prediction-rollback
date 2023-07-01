using System;

namespace UPR.PredictionRollback
{
    public class RepeatPrediction<TCommand> : CommandTimelineDecorator<TCommand> where TCommand : IEquatable<TCommand>
    {
        public RepeatPrediction(ICommandTimeline<TCommand> commandTimeline) : base(commandTimeline)
        {
        }

        public override int GetLatestTickWithCommandBefore(int tickInclusive)
        {
            if (CommandTimeline.GetLatestTickWithCommandBefore(tickInclusive) == -1)
                return -1;

            return tickInclusive;
        }

        public override bool HasCommand(int tick)
        {
            return CommandTimeline.GetLatestTickWithCommandBefore(tick) != -1;
        }

        public override bool HasExactCommand(int tick, TCommand command)
        {
            return HasCommand(tick) && GetCommand(tick).Equals(command);
        }

        public override TCommand GetCommand(int tick)
        {
            int lastTickWithCommand = CommandTimeline.GetLatestTickWithCommandBefore(tick);
            return CommandTimeline.GetCommand(lastTickWithCommand);
        }
    }
}
