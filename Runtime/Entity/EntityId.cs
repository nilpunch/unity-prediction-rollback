using System;

namespace UPR
{
    public readonly struct EntityId : IEquatable<EntityId>
    {
        public int Id { get; }

        public EntityId(int id)
        {
            Id = id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(EntityId other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            return obj is EntityId other && Equals(other);
        }
    }
}
