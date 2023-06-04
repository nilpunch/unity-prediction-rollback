﻿namespace UPR
{
    public interface IEntityCommandTimeline<TCommand>
    {
        int GetLatestTickWithCommand(int tick);
        void ExecuteCommand(int tick);
        void RemoveAllCommandsDownTo(int tick);
        void RemoveCommand(int tick);
        void InsertCommand(int tick, in TCommand command);
    }
}
