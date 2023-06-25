using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    /// <summary>
    /// Code reusal for <see cref="ICommandTimeline{TCommand}"/> decorators.
    /// </summary>
    public class CommandTimelineDecorator<TCommand> : ICommandTimeline<TCommand>
    {
        protected CommandTimelineDecorator(ICommandTimeline<TCommand> commandTimeline)
        {
            CommandTimeline = commandTimeline;
        }

        protected ICommandTimeline<TCommand> CommandTimeline { get; }

        public IReadOnlyList<TCommand> SolidUnpredictedCommands => CommandTimeline.SolidUnpredictedCommands;

        public virtual int GetLatestTickWithSolidCommandBefore(int tickInclusive)
        {
            return CommandTimeline.GetLatestTickWithSolidCommandBefore(tickInclusive);
        }

        public virtual bool HasCommand(int tick)
        {
            return CommandTimeline.HasCommand(tick);
        }

        public virtual bool HasSameCommand(int tick, TCommand command)
        {
            return CommandTimeline.HasSameCommand(tick, command);
        }

        public virtual TCommand GetCommand(int tick)
        {
            return CommandTimeline.GetCommand(tick);
        }

        public void RemoveAllCommandsInRange(int fromTickInclusive, int toTickInclusive)
        {
            CommandTimeline.RemoveAllCommandsInRange(fromTickInclusive, toTickInclusive);
        }

        public void RemoveAllCommandsDownTo(int tick)
        {
            CommandTimeline.RemoveAllCommandsDownTo(tick);
        }

        public void RemoveCommand(int tick)
        {
            CommandTimeline.RemoveCommand(tick);
        }

        public void InsertCommand(int tick, in TCommand command)
        {
            CommandTimeline.InsertCommand(tick, command);
        }
    }
}
