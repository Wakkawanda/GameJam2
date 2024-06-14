using UnityEngine;

namespace Player
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        private PlayerInput _playerInput;
        private Vector2 _direction;

        public void Construct(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        private void Update()
        {
            _direction = _playerInput.Player.Move.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.MovePosition(_direction * _speed * Time.fixedDeltaTime);
        }
    }
}