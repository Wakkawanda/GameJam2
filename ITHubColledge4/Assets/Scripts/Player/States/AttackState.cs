using Scripts;
using UniRx;
using UnityEngine;

namespace States
{
    public class AttackState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;
        private readonly Weapon _weapon;

        private Vector2 _direction;

        public AttackState(Player player, PlayerInput playerInput, Weapon weapon) : base(player)
        {
            _playerInput = playerInput;
            _weapon = weapon;
        }

        public override void OnEnter()
        {
            //todo animation attack
            _weapon.IsAttack = true;
            _weapon.Attack();
            Debug.Log("Attack");
        }

        public override void OnUpdate()
        {
            if (!_weapon.IsAttack)
            {
                Player.ChangeState(Player.MovingState);
            }
        }

        public override void OnExit()
        {
        }
    }
}