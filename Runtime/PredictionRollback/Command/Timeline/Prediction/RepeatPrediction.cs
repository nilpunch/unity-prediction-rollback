using System;

namespace UPR.PredictionRollback
{
    public class RepeatPrediction<TCommand> : CommandTimelineDecorator<TCommand> where TCommand : IEquatable<TCommand>
    {
        public RepeatPrediction(ICommandTimeline<TCommand> commandTimeline) : base(commandTimeline)
        {
        }

        public override bool HasCommand(int tick)
        {
            return CommandTimeline.GetLatestTickWithSolidCommandBefore(tick) != -1;
        }

        public override bool HasExactCommand(int tick, TCommand command)
        {
            return HasCommand(tick) && GetCommand(tick).Equals(command);
        }

        public override TCommand GetCommand(int tick)
        {
            int lastTickWithCommand = CommandTimeline.GetLatestTickWithSolidCommandBefore(tick);
            return CommandTimeline.GetCommand(lastTickWithCommand);
        }
    }
}
