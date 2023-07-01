using System;
using System.Buffers;
using System.Collections.Generic;

namespace UPR.Common
{
    public static class ListExtensions
    {
        public static void RemoveBySwap<T>(this List<T> list, int index)
        {
            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }

        public static void RemoveBySwap<T>(this List<T> list, T item)
        {
            int index = list.IndexOf(item);
            RemoveBySwap(list, index);
        }

        public static void RemoveBySwap<T>(this List<T> list, Predicate<T> predicate)
        {
            int index = list.FindIndex(predicate);
            RemoveBySwap(list, index);
        }

        public static void RadixSort<T>(this List<T> list, Func<T, uint> orderBy)
        {
            RadixCountBytesSort(list, orderBy);
        }

        private static void RadixCountBytesSort<T>(List<T> list, Func<T, uint> orderOf)
        {
            T[] buffer = ArrayPool<T>.Shared.Rent(list.Count);
            int bytesAmount = sizeof(uint);
            for (int i = 0; i < bytesAmount; i++)
            {
                CountSort8Bits(list, buffer, list.Count, i * 8, orderOf);
            }
            ArrayPool<T>.Shared.Return(buffer);
        }

        private static void CountSort8Bits<T>(List<T> input, T[] buffer, int size, int shift, Func<T, uint> orderOf)
        {
            int[] count = new int[256];
            for (int i = 0; i < size; i++)
                count[(orderOf(input[i]) >> shift) & 255]++;

            for (int i = 1; i < 256; i++)
                count[i] += count[i - 1];

            for (int i = size - 1; i >= 0; i--)
                buffer[--count[(orderOf(input[i]) >> shift) & 255]] = input[i];

            for (int i = 0; i < size; i++)
                input[i] = buffer[i];
        }
    }
}
