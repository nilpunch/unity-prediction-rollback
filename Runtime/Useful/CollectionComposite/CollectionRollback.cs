using System;
using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class CollectionRollback : IRollback
    {
        private readonly IReadOnlyContainer<IRollback> _container;

        public CollectionRollback(IReadOnlyContainer<IRollback> container)
        {
            _container = container;
        }

        public void Rollback(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps));

            for (int index = 0; index < _container.Entries.Count; index++)
            {
                _container.Entries[index].Rollback(steps);
            }
        }
    }
}
