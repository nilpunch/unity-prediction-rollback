using UPR.Serialization;

namespace UPR.Tests
{
    public class IncreaseValueCommandDeserializer : IDeserializer<IncreaseValueCommand>
    {
        public IncreaseValueCommand Deserialize(ReadHandle readHandle)
        {
            return new IncreaseValueCommand(readHandle.ReadInt());
        }
    }
}
