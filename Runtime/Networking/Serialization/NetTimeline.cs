using UPR.Common;

namespace UPR.Networking
{
    public struct NetTimeline<TCommand>
    {
        public int RewriteFromTick;
        public int RewriteToTick;
        public ICollection<NetCommand<TCommand>> NetCommands;
    }
}
