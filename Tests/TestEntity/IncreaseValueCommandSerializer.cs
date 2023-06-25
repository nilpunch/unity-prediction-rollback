using UPR.Serialization;

namespace UPR.Tests
{
    public class IncreaseValueCommandSerializer : ISerializer<IncreaseValueCommand>
    {
        public void Serialize(WriteHandle writeHandle, IncreaseValueCommand value)
        {
            writeHandle.WriteInt(value.Delta);
        }
    }
}
