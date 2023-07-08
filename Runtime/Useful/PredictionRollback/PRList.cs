using System;
using System.Collections;
using System.Collections.Generic;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class PRList<T> : IList<T>, IHistory, IRollback, IRebase
    {
        private struct EquatableList : IEquatable<EquatableList>
        {
            public List<T> List;

            public bool Equals(EquatableList other)
            {
                return ReferenceEquals(List, other.List);
            }
        }

        private readonly RarelyChangingValue<EquatableList> _list;

        private List<T> List => _list.Value.List;

        public PRList()
        {
            _list = new RarelyChangingValue<EquatableList>(new EquatableList() {List = new List<T>()});
        }

        public PRList(IEnumerable<T> range)
        {
            _list = new RarelyChangingValue<EquatableList>(new EquatableList() {List = new List<T>(range)});
        }

        public void SaveStep()
        {
            _list.SaveStep();
        }

        public void Rollback(int steps)
        {
            _list.Rollback(steps);
        }

        public void ForgetFromBeginning(int steps)
        {
            _list.ForgetFromBeginning(steps);
        }

        public int Count => List.Count;

        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get => List[index];
            set =>  List[index] = value;
        }

        public void Add(T item)
        {
            List.Add(item);
        }

        public bool Remove(T item)
        {
            return List.Remove(item);
        }

        public void Insert(int index, T item)
        {
            List.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            List.RemoveAt(index);
        }

        public void Clear()
        {
            List.Clear();
        }

        public bool Contains(T item)
        {
            return List.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            List.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            return List.IndexOf(item);
        }
    }
}
