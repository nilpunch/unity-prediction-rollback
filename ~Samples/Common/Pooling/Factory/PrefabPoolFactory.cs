using System.Text;
using UnityEngine;

namespace Tools
{
    public class PrefabPoolFactory<T> : IPoolFactory<T> where T : Object
    {
        private readonly Transform _parent;
        private readonly T _prefab;
        private readonly string _name;
        private readonly StringBuilder _nameBuilder;

        private int _objectIndex = 0;

        public PrefabPoolFactory(T prefab, Transform parent, string name)
        {
            _prefab = prefab;
            _name = name;
            _parent = parent;
            _nameBuilder = new StringBuilder();
        }

        public PrefabPoolFactory(T prefab, Transform parent) : this(prefab, parent, prefab.name)
        {
        }

        public PrefabPoolFactory(T prefab) : this(prefab, null, prefab.name)
        {
        }

        T IPoolFactory<T>.Create()
        {
            T instance = _parent ? Object.Instantiate(_prefab, _parent) : Object.Instantiate(_prefab);
            instance.name = _nameBuilder.Append(_name).Append(' ').Append(_objectIndex).ToString();
            _nameBuilder.Clear();
            _objectIndex++;
            return instance;
        }
    }
}