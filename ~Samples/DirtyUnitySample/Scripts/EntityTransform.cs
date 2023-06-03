using System;
using UnityEngine;

namespace UPR.Samples
{
    public class EntityTransform : MonoBehaviour, IHistory, IRollback, IInitialize
    {
        private ReversibleValue<Vector3> _position;
        private ReversibleValue<Quaternion> _rotation;

        public void Initialize()
        {
            _position = new ReversibleValue<Vector3>(transform.position);
            _rotation = new ReversibleValue<Quaternion>(transform.rotation);
        }

        public void SaveStep()
        {
            _position.Value = transform.position;
            _rotation.Value = transform.rotation;
            _position.SaveStep();
            _rotation.SaveStep();
        }

        public void Rollback(int steps)
        {
            _position.Rollback(steps);
            _rotation.Rollback(steps);
            transform.position = _position.Value;
            transform.rotation = _rotation.Value;
        }
    }
}
