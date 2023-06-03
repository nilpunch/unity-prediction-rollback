using System;
using UnityEngine;

namespace UPR.Samples
{
    public class EntityTransform : FrequentlyChangedComponent<EntityTransform.Memory>
    {
        public struct Memory
        {
            public Quaternion Rotation { get; set; }
            public Vector3 Position { get; set; }
        }

        public Vector3 Position
        {
            get => Data.Position;
            set
            {
                Data.Position = value;
                transform.position = value;
            }
        }

        public Quaternion Rotation
        {
            get => Data.Rotation;
            set
            {
                Data.Rotation = value;
                transform.rotation = value;
            }
        }

        protected override Memory InitialData => new Memory()
        {
            Position = transform.position, Rotation = transform.rotation
        };

        protected override void OnDataChanged()
        {
            transform.position = Data.Position;
            transform.rotation = Data.Rotation;
        }
    }
}
