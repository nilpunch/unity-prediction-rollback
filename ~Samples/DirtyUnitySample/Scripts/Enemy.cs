using UnityEngine;

namespace UPR.Samples
{
    public class Enemy : UnityEntity
    {
        [SerializeField] private DeathHit _deathHit;

        private void Start()
        {
            LocalSimulations.Add(_deathHit);
        }
    }
}
