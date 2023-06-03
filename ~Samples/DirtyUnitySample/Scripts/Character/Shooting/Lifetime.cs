using System;
using UnityEngine;

namespace UPR.Samples
{
    public class Lifetime : RarelyChangedComponent<bool>
    {
        [SerializeField] private bool _aliveInitially = true;

        public bool IsAlive
        {
            get => Data;
            set
            {
                Data = value;
                OnDataChanged();
            }
        }

        protected override bool InitialData => _aliveInitially;
    }
}
