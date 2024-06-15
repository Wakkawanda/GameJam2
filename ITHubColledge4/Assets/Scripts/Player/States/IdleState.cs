using Scripts;
using UnityEngine;

namespace States
{
    public class IdleState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;
        private readonly Animator _animator;

        public IdleState(Player player, PlayerInput playerInput, Animator animator) : base(player)
        {
            _playerInput = playerInput;
            _playerInput = playerInput;
            _animator = animator;
        }

        public override void OnEnter()
        {
            Player.Rigidbody2D.velocity = Vector2.zero;
            _animator.SetBool("isIdle", true);
            Debug.Log("Idle");
        }

        public override void OnUpdate()
        {
            if (_playerInput.Player.Move.ReadValue<Vector2>() != Vector2.zero)
            {
                Player.ChangeState(Player.MovingState);
            }
        }

        public override void OnExit()
        {
            _animator.SetBool("isIdle", false);
        }
    }
}