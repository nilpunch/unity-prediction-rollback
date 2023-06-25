using UPR.Serialization;

namespace UPR.Networking
{
    public class NetCommandSerializer<TCommand> : ISerializer<NetCommand<TCommand>>
    {
        private readonly ISerializer<TCommand> _serializer;

        public NetCommandSerializer(ISerializer<TCommand> serializer)
        {
            _serializer = serializer;
        }

        public void Serialize(WriteHandle writeHandle, NetCommand<TCommand> netCommand)
        {
            writeHandle.WriteInt(netCommand.TargetId.Value);
            writeHandle.WriteInt(netCommand.Tick);
            _serializer.Serialize(writeHandle, netCommand.Command);
        }
    }
}
