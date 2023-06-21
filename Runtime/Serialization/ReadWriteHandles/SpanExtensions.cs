using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace UPR.Serialization
{
    public static class SpanExtensions
    {
        public static void Write(this Span<byte> span, float value)
        {
            int encoded = BitConverter.SingleToInt32Bits(value);

            Write(span, encoded);
        }

        public static void Write(this Span<byte> span, double value)
        {
            long encoded = BitConverter.DoubleToInt64Bits(value);

            Write(span, encoded);
        }

        public static void Write(this Span<byte> span, short value)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            BinaryPrimitives.WriteInt16BigEndian(span, value);
        }

        public static void Write(this Span<byte> span, int value)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            BinaryPrimitives.WriteInt32BigEndian(span, value);
        }

        public static void Write(this Span<byte> span, long value)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            BinaryPrimitives.WriteInt64BigEndian(span, value);
        }

        public static void Write(this Span<byte> span, ushort value)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            BinaryPrimitives.WriteUInt16BigEndian(span, value);
        }

        public static void Write(this Span<byte> span, uint value)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            BinaryPrimitives.WriteUInt32BigEndian(span, value);
        }

        public static void Write(this Span<byte> span, ulong value)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            BinaryPrimitives.WriteUInt64BigEndian(span, value);
        }

        public static float ReadFloat(this ReadOnlySpan<byte> span)
        {
            return BitConverter.Int32BitsToSingle(ReadInt(span));
        }

        public static double ReadDouble(this ReadOnlySpan<byte> span)
        {
            return BitConverter.Int64BitsToDouble(ReadLong(span));
        }

        public static short ReadShort(this ReadOnlySpan<byte> span)
        {
            return BinaryPrimitives.ReadInt16BigEndian(span);
        }

        public static int ReadInt(this ReadOnlySpan<byte> span)
        {
            return BinaryPrimitives.ReadInt32BigEndian(span);
        }

        public static long ReadLong(this ReadOnlySpan<byte> span)
        {
            return BinaryPrimitives.ReadInt64BigEndian(span);
        }

        public static ushort ReadUShort(this ReadOnlySpan<byte> span)
        {
            return BinaryPrimitives.ReadUInt16BigEndian(span);
        }

        public static uint ReadUInt(this ReadOnlySpan<byte> span)
        {
            return BinaryPrimitives.ReadUInt32BigEndian(span);
        }

        public static ulong ReadULong(this ReadOnlySpan<byte> span)
        {
            return BinaryPrimitives.ReadUInt64BigEndian(span);
        }
    }
}
