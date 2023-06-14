using System;
using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class CollectionRollback : IRollback
    {
        private readonly IReadOnlyCollection<IRollback> _collection;

        public CollectionRollback(IReadOnlyCollection<IRollback> collection)
        {
            _collection = collection;
        }

        public void Rollback(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps));

            foreach (var entity in _collection.Entries)
            {
                entity.Rollback(steps);
            }
        }
    }
}
