using Scripts;
using UnityEngine;

namespace States
{
    public class SmokeState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;
        private readonly Animator _animator;

        private Vector2 _direction;

        public SmokeState(Player player, PlayerInput playerInput, Animator animator) : base(player)
        {
            _playerInput = playerInput;
            _animator = animator;
        }

        public override void OnEnter()
        {
            //todo animation attack
            Player.ActiveAttackSmoke = true;
            Debug.Log("SmokeAttack");
            _animator.SetTrigger("Smoke");
            
            Player.Weapon.gameObject.SetActive(false);
        }

        public override void OnUpdate()
        {
        }

        public override void OnExit()
        {
            Player.ActiveAttackSmoke = false;
        }
    }
}