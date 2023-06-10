using System.Runtime.InteropServices;

namespace UPR.Serialization
{
    public static class FastBitConverter
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct DoubleConverter
        {
            [FieldOffset(0)]
            public ulong Long;

            [FieldOffset(0)]
            public double Double;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatConverter
        {
            [FieldOffset(0)]
            public int Int;

            [FieldOffset(0)]
            public float Float;
        }

        public static void WriteToBytes(byte[] bytes, int startIndex, double value)
        {
            DoubleConverter doubleConverter = new DoubleConverter { Double = value };
            WriteWithChosenEndian(bytes, startIndex, doubleConverter.Long);
        }

        public static void WriteToBytes(byte[] bytes, int startIndex, float value)
        {
            FloatConverter floatConverter = new FloatConverter { Float = value };
            WriteWithChosenEndian(bytes, startIndex, floatConverter.Int);
        }

        public static void WriteToBytes(byte[] bytes, int startIndex, short value)
        {
            WriteWithChosenEndian(bytes, startIndex, value);
        }

        public static void WriteToBytes(byte[] bytes, int startIndex, ushort value)
        {
            WriteWithChosenEndian(bytes, startIndex, (short)value);
        }

        public static void WriteToBytes(byte[] bytes, int startIndex, int value)
        {
            WriteWithChosenEndian(bytes, startIndex, value);
        }

        public static void WriteToBytes(byte[] bytes, int startIndex, uint value)
        {
            WriteWithChosenEndian(bytes, startIndex, (int)value);
        }

        public static void WriteToBytes(byte[] bytes, int startIndex, long value)
        {
            WriteWithChosenEndian(bytes, startIndex, (ulong)value);
        }

        public static void WriteToBytes(byte[] bytes, int startIndex, ulong value)
        {
            WriteWithChosenEndian(bytes, startIndex, value);
        }

        private static void WriteWithChosenEndian(byte[] buffer, int offset, ulong data)
        {
#if BIGENDIAN
            buffer[offset + 7] = (byte)(data);
            buffer[offset + 6] = (byte)(data >> 8);
            buffer[offset + 5] = (byte)(data >> 16);
            buffer[offset + 4] = (byte)(data >> 24);
            buffer[offset + 3] = (byte)(data >> 32);
            buffer[offset + 2] = (byte)(data >> 40);
            buffer[offset + 1] = (byte)(data >> 48);
            buffer[offset    ] = (byte)(data >> 56);
#else
            buffer[offset] = (byte)(data);
            buffer[offset + 1] = (byte)(data >> 8);
            buffer[offset + 2] = (byte)(data >> 16);
            buffer[offset + 3] = (byte)(data >> 24);
            buffer[offset + 4] = (byte)(data >> 32);
            buffer[offset + 5] = (byte)(data >> 40);
            buffer[offset + 6] = (byte)(data >> 48);
            buffer[offset + 7] = (byte)(data >> 56);
#endif
        }

        private static void WriteWithChosenEndian(byte[] buffer, int offset, int data)
        {
#if BIGENDIAN
            buffer[offset + 3] = (byte)(data);
            buffer[offset + 2] = (byte)(data >> 8);
            buffer[offset + 1] = (byte)(data >> 16);
            buffer[offset    ] = (byte)(data >> 24);
#else
            buffer[offset] = (byte)(data);
            buffer[offset + 1] = (byte)(data >> 8);
            buffer[offset + 2] = (byte)(data >> 16);
            buffer[offset + 3] = (byte)(data >> 24);
#endif
        }

        private static void WriteWithChosenEndian(byte[] buffer, int offset, short data)
        {
#if BIGENDIAN
            buffer[offset + 1] = (byte)(data);
            buffer[offset    ] = (byte)(data >> 8);
#else
            buffer[offset] = (byte)(data);
            buffer[offset + 1] = (byte)(data >> 8);
#endif
        }
    }
}
