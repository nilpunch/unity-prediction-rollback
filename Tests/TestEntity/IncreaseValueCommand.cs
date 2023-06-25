using System;

namespace UPR.Tests
{
    public struct IncreaseValueCommand : IEquatable<IncreaseValueCommand>
    {
        public IncreaseValueCommand(int delta)
        {
            Delta = delta;
        }

        public int Delta { get; }

        public bool Equals(IncreaseValueCommand other)
        {
            return Delta == other.Delta;
        }

        public override bool Equals(object obj)
        {
            return obj is IncreaseValueCommand other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Delta;
        }
    }
}
