namespace UPR.Serialization
{
    public class FloatDeserializer : IDeserializer<float>
    {
        public float Deserialize(IReadHandle readHandle)
        {
            return readHandle.ReadFloat();
        }
    }
}
