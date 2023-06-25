using UPR.Common;
using UPR.Serialization;

namespace UPR.Useful
{
    public class CollectionDeserializer<TEntry> : IDeserializer<ICollection<TEntry>>
    {
        private readonly IDeserializer<TEntry> _objectDeserializer;

        public CollectionDeserializer(IDeserializer<TEntry> objectDeserializer)
        {
            _objectDeserializer = objectDeserializer;
        }

        public ICollection<TEntry> Deserialize(ReadHandle readHandle)
        {
            int count = readHandle.ReadInt();
            var collection = new Collection<TEntry>(count);
            for (int index = 0; index < count; index++)
            {
                collection.Add(_objectDeserializer.Deserialize(readHandle));
            }
            return collection;
        }
    }
}
