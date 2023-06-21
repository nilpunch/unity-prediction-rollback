using UPR.Serialization;

namespace UPR.Networking
{
    public class SingleCommandMarshal<TCommand> : ICommandMarshal
    {
        private readonly ICommandRouter<TCommand> _router;
        private readonly IDeserializer<TCommand> _deserializer;

        public SingleCommandMarshal(ICommandRouter<TCommand> router, IDeserializer<TCommand> deserializer)
        {
            _router = router;
            _deserializer = deserializer;
        }

        public void DeserializeAndForward(ReadHandle commandData)
        {
            TargetId targetId = new TargetId(commandData.ReadInt());
            int tick = commandData.ReadInt();
            var command = _deserializer.Deserialize(commandData);

            _router.ForwardCommand(command, targetId, tick);
        }
    }
}
