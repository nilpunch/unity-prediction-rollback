using System;

namespace UPR.PredictionRollback
{
    public class CollectionRollback : IRollback
    {
        private readonly ICollection<IRollback> _collection;

        public CollectionRollback(ICollection<IRollback> collection)
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
