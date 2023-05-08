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

        public static bool operator ==(EntityId a, EntityId b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(EntityId a, EntityId b)
        {
            return !(a == b);
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
