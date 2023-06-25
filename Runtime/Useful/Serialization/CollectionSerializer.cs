using UPR.Common;
using UPR.Serialization;

namespace UPR.Useful
{
    public class CollectionSerializer<TEntry> : ISerializer<ICollection<TEntry>>
    {
        private readonly ISerializer<TEntry> _serializer;

        public CollectionSerializer(ISerializer<TEntry> serializer)
        {
            _serializer = serializer;
        }

        public void Serialize(WriteHandle writeHandle, ICollection<TEntry> value)
        {
            writeHandle.WriteInt(value.Entries.Count);
            foreach (var entry in value.Entries)
            {
                _serializer.Serialize(writeHandle, entry);
            }
        }
    }
}
