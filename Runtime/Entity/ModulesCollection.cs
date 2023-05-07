using System;
using System.Collections.Generic;

namespace UPR
{
    public class ModulesCollection
    {
        private readonly Dictionary<Type, object> _modulesByType = new Dictionary<Type, object>();

        public void AddModule<TModule>(TModule module)
        {
            _modulesByType.Add(typeof(TModule), module);
        }

        public bool TryFindModule<TModule>(out TModule result)
        {
            var moduleType = typeof(TModule);
            result = default;

            bool isFounded = _modulesByType.TryGetValue(moduleType, out var module);
            if (isFounded)
            {
                result = (TModule)module;
                return true;
            }

            return false;
        }
    }
}
