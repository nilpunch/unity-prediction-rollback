using System;

namespace UPR.Serialization
{
    public ref struct WriteHandle
    {
        private readonly byte[] _data;

        private int _index;

        public WriteHandle(byte[] data, int startOffset)
        {
            _data = data;
            _index = startOffset;
        }

        public bool IsEnded => _index < _data.Length;

        public void WriteInt(int value)
        {
            GetSpan(4).Write(value);
        }

        public void WriteFloat(float value)
        {
            GetSpan(4).Write(value);
        }

        private Span<byte> GetSpan(int size)
        {
            var span = new Span<byte>(_data, _index, size);
            _index += size;
            return span;
        }
    }
}
