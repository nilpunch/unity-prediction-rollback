namespace UPR.Networking
{
    public interface ICommandRouter<TCommand>
    {
        public void ForwardCommand(TargetId targetId, TCommand command, int tick);
    }
}
