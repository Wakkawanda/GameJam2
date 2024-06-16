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
            Vector2 direction = _playerInput.Player.Move.ReadValue<Vector2>() * 10;
            Player.Rigidbody2D.MovePosition(Player.Rigidbody2D.position + direction * Player.Speed / 10 * Time.fixedDeltaTime);
        }

        public override void OnExit()
        {
        }
    }
}