namespace UPR.Networking
{
    public struct NetCommand<TCommand>
    {
        public TargetId TargetId;
        public int Tick;
        public TCommand Command;
    }
}