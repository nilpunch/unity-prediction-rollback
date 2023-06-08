using System;
using UnityEngine;
using UPR.PredictionRollback;
using UPR.Utils;

namespace UPR.Samples
{
    public class EntityTransform : MonoBehaviour, IInitialize, IHistory, IRollback, IRebase
    {
        private FrequentlyChangingValue<Vector3> _position;
        private FrequentlyChangingValue<Quaternion> _rotation;

        public void Initialize()
        {
            _position = new FrequentlyChangingValue<Vector3>(transform.position);
            _rotation = new FrequentlyChangingValue<Quaternion>(transform.rotation);
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

        public void ForgetFromBeginning(int steps)
        {
            _position.ForgetFromBeginning(steps);
            _rotation.ForgetFromBeginning(steps);
        }
    }
}
