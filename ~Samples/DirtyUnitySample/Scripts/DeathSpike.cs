using UnityEngine;

namespace UPR.Samples
{
    public class DeathSpike : UnityEntity
    {
        [SerializeField] private DeathRaycast _deathRaycast;

        private void Start()
        {
            LocalSimulations.AddSimulation(_deathRaycast);
        }
    }
}
