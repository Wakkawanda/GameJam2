using Scripts;
using UnityEngine;

namespace States
{
    public class JumpFranticState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;
        private readonly Animator _animator;

        public JumpFranticState(Player player, PlayerInput playerInput, Animator animator) : base(player)
        {
            _playerInput = playerInput;
            _animator = animator;
        }

        public override void OnEnter()
        {
            //todo animation attack
            Debug.Log("JumpFranticState");
            _animator.SetTrigger("JumpFrantic");
            Player.Weapon.gameObject.SetActive(false);
        }

        public override void OnUpdate()
        {
        }

        public override void OnExit()
        {
        }
    }
}