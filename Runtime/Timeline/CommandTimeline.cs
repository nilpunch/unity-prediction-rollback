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

        public void RemoveCommand(int tick, EntityId entityId)
        {
            _timeline[tick].RemoveAll(command => command.Entity == entityId);
        }

        public void InsertCommand(int tick, in TCommand command, EntityId entityId)
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

        public void ExecuteCommands(int tick)
        {
            if (_timeline.TryGetValue(tick, out var commands))
            {
                foreach (var command in commands)
                {
                    _commandRouter.ForwardCommand(command.Command, command.Entity);
                }
            }
        }
    }
}
