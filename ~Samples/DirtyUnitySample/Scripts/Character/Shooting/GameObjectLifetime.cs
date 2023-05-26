using UnityEngine;

namespace UPR.Samples
{
    public class GameObjectLifetime : Lifetime
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Collider _collider;

        public override void OnValueChanged()
        {
            _renderer.enabled = IsAlive;
            _collider.enabled = IsAlive;
        }
    }
}
