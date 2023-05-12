using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace UPR.Samples
{
    public class UnitySimulation : MonoBehaviour
    {
        [SerializeField] private int _ticksPerSecond = 30;

        public static SimulationSpeed SimulationSpeed { get; private set; }

        public static TimeTravelMachine TimeTravelMachine { get; private set; }
        public static ICommandTimeline<CharacterMoveCommand> CharacterMovement { get; private set; }
        public static ICommandTimeline<CharacterShootCommand> CharacterShooting { get; private set; }

        public static IEntityWorld<IEntity> EntityWorld { get; private set; }

        public static float ElapsedTime { get; private set; }

        public static IdGenerator IdGenerator { get; private set; }
        public static int CurrentTick => Mathf.FloorToInt(ElapsedTime * SimulationSpeed.TicksPerSecond);

        private void Start()
        {
            var charactersEntityWorld = new EntityWorld<IEntity>();
            EntityWorld = charactersEntityWorld;

            SimulationSpeed = new SimulationSpeed(_ticksPerSecond);
            TimeTravelMachine = new TimeTravelMachine(charactersEntityWorld, charactersEntityWorld, charactersEntityWorld);
            CharacterMovement = new CommandTimeline<CharacterMoveCommand>(
                new CommandRouter<CharacterMoveCommand>(new EntityFinderAdapter<IEntity, Character>(EntityWorld)));
            CharacterShooting = new CommandTimeline<CharacterShootCommand>(
                new CommandRouter<CharacterShootCommand>(new EntityFinderAdapter<IEntity, Character>(EntityWorld)));

            TimeTravelMachine.AddCommandsTimeline(CharacterMovement);
            TimeTravelMachine.AddCommandsTimeline(CharacterShooting);

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

            TimeTravelMachine.FastForwardToTick(CurrentTick);
        }
    }
}
