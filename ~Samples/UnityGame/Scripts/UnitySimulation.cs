using System;
using System.Collections;
using UnityEngine;

namespace UPR.Samples
{
    public class UnitySimulation : MonoBehaviour
    {
        [SerializeField] private int _ticksPerSecond = 30;

        public static SimulationSpeed SimulationSpeed { get; private set; }

        public WorldTimeline WorldTimeline { get; private set; }

        public float ElapsedTime {get; private set; }
        public int CurrentTick => Mathf.FloorToInt(ElapsedTime * SimulationSpeed.TicksPerSecond);

        private void Start()
        {
            var idGenerator = new UniqueIdGenerator();
            var entityWorld = new EntityWorld();

            SimulationSpeed = new SimulationSpeed(_ticksPerSecond);
            WorldTimeline = new WorldTimeline(entityWorld, entityWorld, entityWorld);

            WorldTimeline.RegisterTimeline(
                new CommandTimeline<CharacterMoveCommand>(
                    new CommandRouter<CharacterMoveCommand>(entityWorld)));

            foreach (UnityEntity unityEntity in FindObjectsOfType<UnityEntity>())
            {
                unityEntity.Id = idGenerator.Generate();
                entityWorld.RegisterEntityAtStep(-1, unityEntity);
            }
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime;

            WorldTimeline.FastForwardToTick(CurrentTick);
        }
    }
}
