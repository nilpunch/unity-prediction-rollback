using System;
using System.Collections;
using UnityEngine;

namespace UPR.Samples
{
    public class UnitySimulation : MonoBehaviour
    {
        [SerializeField] private int _ticksPerSecond = 30;

        public static SimulationSpeed SimulationSpeed { get; private set; }

        public static WorldTimeline WorldTimeline { get; private set; }

        public static EntityWorld EntityWorld { get; private set; }

        public static float ElapsedTime { get; private set; }

        public static IdGenerator IdGenerator { get; private set; }
        public static int CurrentTick => Mathf.FloorToInt(ElapsedTime * SimulationSpeed.TicksPerSecond);

        private void Start()
        {
            EntityWorld = new EntityWorld();

            SimulationSpeed = new SimulationSpeed(_ticksPerSecond);
            WorldTimeline = new WorldTimeline(EntityWorld, EntityWorld, EntityWorld);

            WorldTimeline.RegisterTimeline(
                new CommandTimeline<CharacterMoveCommand>(
                    new CommandRouter<CharacterMoveCommand>(EntityWorld)));

            int startingEntities = 0;
            foreach (UnityEntity unityEntity in FindObjectsOfType<UnityEntity>())
            {
                unityEntity.Id = new EntityId(startingEntities);
                EntityWorld.RegisterEntityAtStep(-1, unityEntity);

                startingEntities += 1;
            }

            IdGenerator = new IdGenerator(startingEntities);
            EntityWorld.RegisterEntityAtStep(-1, IdGenerator);

            EntityWorld.SubmitEntities();
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime;

            WorldTimeline.FastForwardToTick(CurrentTick);
        }
    }
}