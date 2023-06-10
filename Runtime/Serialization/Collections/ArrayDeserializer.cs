namespace UPR.Serialization
{
    public class ArrayDeserializer<TObject> : IDeserializer<TObject[]>
    {
        private readonly IDeserializer<int> _lengthField = new IntDeserializer();
        private readonly IDeserializer<TObject> _objectDeserializer;

        public ArrayDeserializer(IDeserializer<TObject> objectDeserializer)
        {
            _objectDeserializer = objectDeserializer;
        }

        public TObject[] Deserialize(IReadHandle readHandle)
        {
            int length = _lengthField.Deserialize(readHandle);
            TObject[] result = new TObject[length];
            for (int index = 0; index < result.Length; index++)
            {
                result[index] = _objectDeserializer.Deserialize(readHandle);
            }
            return result;
        }
    }
}
