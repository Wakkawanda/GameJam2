using UnityEngine;
using Zenject;

namespace Player
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        private PlayerInput _playerInput;
        private Vector2 _direction;

        [Inject]
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
            _rigidbody2D.MovePosition(_rigidbody2D.position + _direction * _speed * Time.fixedDeltaTime);
        }
    }
}