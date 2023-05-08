using System.Collections.Generic;

namespace UPR
{
    public class CommandTimeline<TCommand> : ICommandTimeline<TCommand>
    {
        private readonly ICommandRouter<TCommand> _commandRouter;
        private readonly Dictionary<long, List<EntityCommand<TCommand>>> _timeline;

        public CommandTimeline(ICommandRouter<TCommand> commandRouter)
        {
            _commandRouter = commandRouter;
        }

        public void RemoveCommand(long tick, EntityId entityId)
        {
            _timeline[tick].RemoveAll(command => command.Target == entityId);
        }

        public void InsertCommand(long tick, in TCommand command, EntityId entityId)
        {
            if (_timeline.TryGetValue(tick, out var commands))
            {
                commands.Add(new EntityCommand<TCommand>(command, entityId));
            }
            else
            {
                _timeline.Add(tick, new List<EntityCommand<TCommand>>()
                {
                    new EntityCommand<TCommand>(command, entityId)
                });
            }
        }

        public void ExecuteCommands(long tick)
        {
            if (_timeline.TryGetValue(tick, out var commands))
            {
                foreach (var command in commands)
                {
                    _commandRouter.ForwardCommand(command.Command, command.Target);
                }
            }
        }
    }
}
