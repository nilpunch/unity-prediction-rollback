namespace UPR.Networking
{
    public interface ICommandRouter<TCommand>
    {
        public void ForwardCommand(TCommand command, TargetId targetId, int tick);
    }
}
