namespace UPR
{
    public struct Checksum
    {
        public long Value { get; }

        public Checksum(long value)
        {
            Value = value;
        }

        public static Checksum Combine(Checksum a, Checksum b)
        {
            return new Checksum(a.Value ^ (b.Value + 0x9e3779b9 + (a.Value << 6) + (a.Value >> 2)));
        }
    }
}
