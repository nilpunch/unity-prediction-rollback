using System;
using System.Collections;
using Tools;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace UPR.Samples
{
    public class UnitySimulation : MonoBehaviour
    {
        [SerializeField] private int _ticksPerSecond = 30;
        [SerializeField] private Bullet _bulletPrefab;

        public static SimulationSpeed SimulationSpeed { get; private set; }

        public static TimeTravelMachine TimeTravelMachine { get; private set; }
        public static ICommandTimeline<CharacterMoveCommand> CharacterMovement { get; private set; }
        public static ICommandTimeline<CharacterShootCommand> CharacterShooting { get; private set; }

        public static IEntityWorld<Character> CharacterWorld { get; private set; }
        public static IEntityWorld<DeathSpike> DeathSpikeWorld { get; private set; }
        public static IEntityWorld<Bullet> BulletsWorld { get; private set; }

        public static IFactory<Bullet> BulletsFactory { get; private set; }

        public static float ElapsedTime { get; private set; }

        public static IdGenerator IdGenerator { get; private set; }
        public static int CurrentTick => Mathf.FloorToInt(ElapsedTime * SimulationSpeed.TicksPerSecond);

        private void Start()
        {
            SimulationSpeed = new SimulationSpeed(_ticksPerSecond);

            var charactersWorld = new EntityWorld<Character>();
            CharacterWorld = charactersWorld;
            var bulletWorld = new EntityWorld<Bullet>();
            BulletsWorld = bulletWorld;
            var deathSpikeWorld = new EntityWorld<DeathSpike>();
            DeathSpikeWorld = deathSpikeWorld;

            int entityIndex = 0;
            foreach (UnityEntity unityEntity in FindObjectsOfType<UnityEntity>())
            {
                unityEntity.Id = new EntityId(entityIndex);
                entityIndex += 1;

                switch (unityEntity)
                {
                    case Character character:
                        CharacterWorld.RegisterEntityAtStep(-1, character);
                        break;
                    case DeathSpike deathSpike:
                        DeathSpikeWorld.RegisterEntityAtStep(-1, deathSpike);
                        break;
                }
            }

            CharacterWorld.SubmitRegistration();
            DeathSpikeWorld.SubmitRegistration();

            IdGenerator = new IdGenerator(entityIndex);

            var bulletsFactory = new EntityFactory<Bullet>(bulletWorld, IdGenerator, new PrefabFactory<Bullet>(_bulletPrefab));
            BulletsFactory = bulletsFactory;

            var worldReversibleHistory = new ReversibleHistories();
            worldReversibleHistory.AddHistory(new ReversibleHistoryAdapter(IdGenerator, IdGenerator));
            worldReversibleHistory.AddHistory(new ReversibleHistoryAdapter(charactersWorld, charactersWorld));
            worldReversibleHistory.AddHistory(new ReversibleHistoryAdapter(bulletWorld, bulletWorld));
            worldReversibleHistory.AddHistory(new ReversibleHistoryAdapter(deathSpikeWorld, deathSpikeWorld));

            var worldSimulation = new Simulations();
            worldSimulation.AddSimulation(IdGenerator);
            worldSimulation.AddSimulation(charactersWorld);
            worldSimulation.AddSimulation(bulletWorld);
            worldSimulation.AddSimulation(deathSpikeWorld);
            worldSimulation.AddSimulation(bulletsFactory);

            TimeTravelMachine = new TimeTravelMachine(worldReversibleHistory, worldSimulation, worldReversibleHistory);

            CharacterMovement = new CommandTimeline<CharacterMoveCommand>(
                new CommandRouter<CharacterMoveCommand>(CharacterWorld));
            CharacterShooting = new CommandTimeline<CharacterShootCommand>(
                new CommandRouter<CharacterShootCommand>(CharacterWorld));

            TimeTravelMachine.AddCommandsTimeline(CharacterMovement);
            TimeTravelMachine.AddCommandsTimeline(CharacterShooting);
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime;

            TimeTravelMachine.FastForwardToTick(CurrentTick);
        }
    }
}
