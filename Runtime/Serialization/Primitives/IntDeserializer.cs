namespace UPR.Serialization
{
    public class IntDeserializer : IDeserializer<int>
    {
        public int Deserialize(IReadHandle readHandle)
        {
            return readHandle.ReadInt();
        }
    }
}
