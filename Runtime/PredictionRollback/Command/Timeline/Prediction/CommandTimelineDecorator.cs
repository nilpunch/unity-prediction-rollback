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

        public IReadOnlyList<TCommand> FilledCommands => CommandTimeline.FilledCommands;

        public virtual int GetLatestTickWithCommandBefore(int tickInclusive)
        {
            return CommandTimeline.GetLatestTickWithCommandBefore(tickInclusive);
        }

        public virtual bool HasCommand(int tick)
        {
            return CommandTimeline.HasCommand(tick);
        }

        public virtual bool HasExactCommand(int tick, TCommand command)
        {
            return CommandTimeline.HasExactCommand(tick, command);
        }

        public virtual TCommand GetCommand(int tick)
        {
            return CommandTimeline.GetCommand(tick);
        }

        public void RemoveAllCommandsInRange(int fromTickInclusive, int toTickInclusive)
        {
            CommandTimeline.RemoveAllCommandsInRange(fromTickInclusive, toTickInclusive);
        }

        public void RemoveAllCommandsDownTo(int tickExclusive)
        {
            CommandTimeline.RemoveAllCommandsDownTo(tickExclusive);
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
