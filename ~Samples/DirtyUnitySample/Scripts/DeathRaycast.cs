using UnityEngine;

namespace UPR.Samples
{
    public class DeathRaycast : MonoBehaviour, ISimulation
    {
        [SerializeField] private Vector3 _dimensions;

        public void StepForward()
        {
            Physics.SyncTransforms();

            var overlaps = Physics.OverlapBox(transform.position, _dimensions / 2f);

            foreach (var overlap in overlaps)
            {
                if (overlap.TryGetComponent(out UnityEntity unityEntity))
                {
                    Debug.Log("Founded!");
                    if (unityEntity.IsAlive)
                    {
                        unityEntity.Kill();
                        Debug.Log("Killed!");
                    }
                }
            }
        }
    }
}
