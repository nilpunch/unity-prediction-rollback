using System;

namespace UPR
{
    public readonly struct EntityId : IEquatable<EntityId>
    {
        public EntityId(int value)
        {
            Value = value;
        }

        public int Value { get; }

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
            return Value.GetHashCode();
        }

        public bool Equals(EntityId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is EntityId other && Equals(other);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
