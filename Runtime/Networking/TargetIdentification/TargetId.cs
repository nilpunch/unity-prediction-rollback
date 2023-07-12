using System;

namespace UPR.Networking
{
    public readonly struct TargetId : IEquatable<TargetId>, IComparable<TargetId>
    {
        public TargetId(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static bool operator ==(TargetId a, TargetId b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TargetId a, TargetId b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(TargetId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is TargetId other && Equals(other);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(TargetId other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}
