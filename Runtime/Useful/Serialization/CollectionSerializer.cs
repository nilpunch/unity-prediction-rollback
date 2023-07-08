using UPR.Common;
using UPR.Serialization;

namespace UPR.Useful
{
    public class CollectionSerializer<TEntry> : ISerializer<IContainer<TEntry>>
    {
        private readonly ISerializer<TEntry> _serializer;

        public CollectionSerializer(ISerializer<TEntry> serializer)
        {
            _serializer = serializer;
        }

        public void Serialize(WriteHandle writeHandle, IContainer<TEntry> value)
        {
            writeHandle.WriteInt(value.Entries.Count);
            foreach (var entry in value.Entries)
            {
                _serializer.Serialize(writeHandle, entry);
            }
        }
    }
}
