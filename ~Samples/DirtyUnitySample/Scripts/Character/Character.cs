using System;
using UnityEngine;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    public class Character : UnityEntity, ICommandPlayer
    {
        [SerializeField] private Lifetime _lifetime;
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private CharacterShooting _characterShooting;

        private ICommandTimeline<CharacterMoveCommand> _moveCommandTimeline;
        private ICommandTimeline<CharacterShootCommand> _shootCommandTimeline;

        public ICommandTimeline<CharacterMoveCommand> MoveCommandTimeline => _moveCommandTimeline;
        public ICommandTimeline<CharacterShootCommand> ShootCommandTimeline => _shootCommandTimeline;

        public override void Initialize()
        {
            base.Initialize();

            _moveCommandTimeline = new PredictionCommandTimeline<CharacterMoveCommand>(new CommandTimeline<CharacterMoveCommand>());
            _shootCommandTimeline = new PredictionCommandTimeline<CharacterShootCommand>(new CommandTimeline<CharacterShootCommand>());
        }

        public void ExecuteCommands(int tick)
        {
            if (!_lifetime.IsAlive)
                return;

            if (_moveCommandTimeline.HasCommand(tick))
            {
                var command = _moveCommandTimeline.GetCommand(tick);
                _characterMovement?.SetMoveDirection(command.MoveDirection);
            }

            if (_shootCommandTimeline.HasCommand(tick))
            {
                var command = _shootCommandTimeline.GetCommand(tick);

                _characterShooting.SetShootingDirection(command.Direction);
                if (command.IsShooting)
                {
                    _characterShooting.EnableShooting();
                }
                else
                {
                    _characterShooting.DisableShooting();
                }
            }
        }
    }
}
