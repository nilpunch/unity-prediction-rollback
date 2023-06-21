namespace UPR.Serialization
{
    public class ArrayDeserializer<TObject> : IDeserializer<TObject[]>
    {
        private readonly IDeserializer<TObject> _objectDeserializer;

        public ArrayDeserializer(IDeserializer<TObject> objectDeserializer)
        {
            _objectDeserializer = objectDeserializer;
        }

        public TObject[] Deserialize(ReadHandle readHandle)
        {
            int length = readHandle.ReadInt();
            TObject[] result = new TObject[length];
            for (int index = 0; index < result.Length; index++)
            {
                result[index] = _objectDeserializer.Deserialize(readHandle);
            }
            return result;
        }
    }
}
