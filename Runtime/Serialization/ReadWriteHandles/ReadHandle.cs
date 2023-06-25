using System;

namespace UPR.Serialization
{
    public ref struct ReadHandle
    {
        private readonly byte[] _data;

        private int _index;

        public ReadHandle(byte[] data, int start)
        {
            _data = data;
            _index = start;
        }

        public bool IsEnded => _index < _data.Length;

        public int ReadInt()
        {
            return GetSpan(4).ReadInt();
        }

        public float ReadFloat()
        {
            return GetSpan(4).ReadFloat();
        }

        private ReadOnlySpan<byte> GetSpan(int size)
        {
            var span = new ReadOnlySpan<byte>(_data, _index, size);
            _index += size;
            return span;
        }
    }
}
