using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UPR.Common;
using Debug = UnityEngine.Debug;

namespace UPR.Samples
{
    public class SortingBenchmark : MonoBehaviour
    {
        [SerializeField] private int _elementsCount = 10000000;

        public static IList<T> Shuffle<T>(IList<T> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                Swap(list, i - 1, UnityEngine.Random.Range(i, list.Count));
            }

            return list;
        }

        public static void Swap<T>(IList<T> list, int first, int second)
        {
            (list[first], list[second]) = (list[second], list[first]);
        }

        private void Awake()
        {
            var forQuick = Enumerable.Range(0, _elementsCount).Select(i => (uint)i).ToList();
            var forRadix = Enumerable.Range(0, _elementsCount).Select(i => (uint)i).ToList();

            UnityEngine.Random.InitState(0);
            Shuffle(forQuick);
            UnityEngine.Random.InitState(0);
            Shuffle(forRadix);

            Stopwatch stopwatch = Stopwatch.StartNew();
            forQuick.Sort((a, b) => (int)a - (int)b);
            stopwatch.Stop();
            Debug.Log("Quick sort: " + stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            forRadix.RadixSort(a => a);
            stopwatch.Stop();
            Debug.Log("Radix sort: " + stopwatch.ElapsedMilliseconds);

            for (int i = 0; i < 10; i++)
            {
                Debug.Log("Quick: " + forQuick[i]);
            }

            for (int i = 0; i < 10; i++)
            {
                Debug.Log("Radix: " + forRadix[i]);
            }
        }
    }
}
