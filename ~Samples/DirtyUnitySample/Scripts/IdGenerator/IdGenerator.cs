using UPR.PredictionRollback;
using UPR.Utils;

namespace UPR.Samples
{
    public class IdGenerator : Entity, IIdGenerator
    {
        private readonly RarelyChangingValue<int> _idCounter;

        public IdGenerator(int startId)
        {
            _idCounter = new RarelyChangingValue<int>(startId);
            LocalHistories.Add(_idCounter);
            LocalRollbacks.Add(_idCounter);
            LocalRebases.Add(_idCounter);
        }

        public EntityId Generate()
        {
            int id = _idCounter.Value;
            _idCounter.Value += 1;
            return new EntityId(id);
        }
    }
}
