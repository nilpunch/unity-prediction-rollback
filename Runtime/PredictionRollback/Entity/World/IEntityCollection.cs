using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public interface IEntityCollection<out TEntity>
    {
        IReadOnlyList<TEntity> Entities { get; }
    }
}
