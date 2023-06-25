using UPR.PredictionRollback;
using UPR.Serialization;

namespace UPR.Networking
{
    public class NetCommandDeserializer<TCommand> : IDeserializer<NetCommand<TCommand>>
    {
        private readonly IDeserializer<TCommand> _deserializer;

        public NetCommandDeserializer(IDeserializer<TCommand> deserializer)
        {
            _deserializer = deserializer;
        }

        public NetCommand<TCommand> Deserialize(ReadHandle readHandle)
        {
            TargetId targetId = new TargetId(readHandle.ReadInt());
            int tick = readHandle.ReadInt();
            TCommand command = _deserializer.Deserialize(readHandle);

            return new NetCommand<TCommand>()
            {
                TargetId = targetId,
                Tick = tick,
                Command = command
            };
        }
    }

    public class NetTimelineSerializer<TCommand> : ISerializer<NetTimeline<TCommand>>
    {
        public void Serialize(WriteHandle writeHandle, NetTimeline<TCommand> value)
        {
            throw new System.NotImplementedException();
        }
    }

    public class NetTimelineDeserializer<TCommand> : IDeserializer<NetTimeline<TCommand>>
    {
        public NetTimeline<TCommand> Deserialize(ReadHandle readHandle)
        {
            throw new System.NotImplementedException();
        }
    }
}
