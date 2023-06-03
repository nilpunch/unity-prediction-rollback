using UnityEngine;

namespace UPR.Samples
{
    public class GameObjectLifetime : Lifetime
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Collider _collider;

        protected override void OnAliveChanged()
        {
            base.OnAliveChanged();

            _renderer.enabled = IsAlive;
            _collider.enabled = IsAlive;
        }
    }
}
