using System.Collections.Generic;

namespace UPR
{
    public class CommandTimeline<TCommand> : ICommandTimeline<TCommand>
    {
        private readonly ICommandRouter<TCommand> _commandRouter;
        private readonly Dictionary<long, List<EntityCommand<TCommand>>> _timeline = new Dictionary<long, List<EntityCommand<TCommand>>>();

        public CommandTimeline(ICommandRouter<TCommand> commandRouter)
        {
            _commandRouter = commandRouter;
        }

        public int EarliestCommandChange { get; private set; }

        public void ApproveChangesUpTo(int tick)
        {
            EarliestCommandChange = tick;
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

        public void RemoveAllDownTo(int tick)
        {
            foreach (var tickCommandPair in _timeline)
            {
                if (tickCommandPair.Key >= tick)
                {
                    tickCommandPair.Value.Clear();
                }
            }

            if (tick < EarliestCommandChange)
            {
                EarliestCommandChange = tick;
            }
        }

        public void RemoveAllCommands(int tick)
        {
            if (_timeline.TryGetValue(tick, out var commands))
            {
                commands.Clear();

                if (tick < EarliestCommandChange)
                {
                    EarliestCommandChange = tick;
                }
            }
        }

        public void RemoveCommand(int tick, EntityId entityId)
        {
            if (_timeline.TryGetValue(tick, out var commands))
            {
                commands.RemoveAll(command => command.Entity == entityId);

                if (tick < EarliestCommandChange)
                {
                    EarliestCommandChange = tick;
                }
            }
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

            if (tick < EarliestCommandChange)
            {
                EarliestCommandChange = tick;
            }
        }
    }
}
