using UnityEngine;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    public class DeathHit : MonoBehaviour, ISimulation
    {
        [SerializeField] private Vector3 _dimensions;

        private readonly Collider[] _castResults = new Collider[1000];

        public void StepForward()
        {
            int overlaps = Physics.OverlapBoxNonAlloc(transform.position, _dimensions / 2f, _castResults);

            for (int i = 0; i < overlaps; i++)
            {
                var result = _castResults[i];

                if (result.TryGetComponent(out Lifetime lifetime) && lifetime.IsAlive)
                {
                    lifetime.IsAlive = false;
                }
            }
        }
    }
}
