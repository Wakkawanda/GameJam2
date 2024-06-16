using Scripts;
using UnityEngine;

namespace States
{
    public class VortexState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;
        private readonly Animator _animator;

        public VortexState(Player player, PlayerInput playerInput, Animator animator) : base(player)
        {
            _playerInput = playerInput;
            _animator = animator;
        }

        public override void OnEnter()
        {
            //todo animation attack
            Debug.Log("VortexState");
            _animator.SetTrigger("Vortex");
            Player.Weapon.gameObject.SetActive(false);
            Player.ActiveAttackVortex = true;
        }

        public override void OnUpdate()
        {
            Vector2 direction = _playerInput.Player.Move.ReadValue<Vector2>() * 10;
            Player.Rigidbody2D.MovePosition(Player.Rigidbody2D.position + direction * Player.Speed / 10 * Time.fixedDeltaTime);
        }

        public override void OnExit()
        {
            Player.ActiveAttackVortex = false;
        }
    }
}