using UnityEngine;

namespace UPR.Samples
{
    public class EntityTransform : MonoBehaviour, IMemory<EntityTransform.Memory>
    {
        public struct Memory
        {
            public Quaternion Rotation { get; set; }
            public Vector3 Position { get; set; }
        }

        private Memory _memory;

        public Vector3 Position
        {
            get => _memory.Position;
            set
            {
                _memory.Position = value;
                transform.position = value;
            }
        }

        public Quaternion Rotation
        {
            get => _memory.Rotation;
            set
            {
                _memory.Rotation = value;
                transform.rotation = value;
            }
        }

        private void Awake()
        {
            _memory.Position = transform.position;
            _memory.Rotation = transform.rotation;
        }

        public Memory Save()
        {
            return _memory;
        }

        public void Load(Memory memory)
        {
            Position = memory.Position;
            Rotation = memory.Rotation;
        }
    }
}
