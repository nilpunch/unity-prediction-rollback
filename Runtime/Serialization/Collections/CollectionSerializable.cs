using System.Collections.Generic;

namespace UPR.Serialization
{
    public class CollectionSerializable : ISerializable
    {
        private readonly IReadOnlyCollection<ISerializable> _serializableCollection;

        public CollectionSerializable(IReadOnlyCollection<ISerializable> serializableCollection)
        {
            _serializableCollection = serializableCollection;
        }

        public void Serialize(IWriteHandle writeHandle)
        {
            new IntSerializable(_serializableCollection.Count).Serialize(writeHandle);
            foreach (var serialization in _serializableCollection)
            {
                serialization.Serialize(writeHandle);
            }
        }
    }
}
