using System;

namespace UPR.Networking
{
    public readonly struct CommandTimelineId : IEquatable<CommandTimelineId>, IComparable<CommandTimelineId>
    {
        public CommandTimelineId(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static bool operator ==(CommandTimelineId a, CommandTimelineId b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(CommandTimelineId a, CommandTimelineId b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(CommandTimelineId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is CommandTimelineId other && Equals(other);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(CommandTimelineId other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}
