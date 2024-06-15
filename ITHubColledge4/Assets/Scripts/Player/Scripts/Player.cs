using System.Collections.Generic;
using System.Linq;
using States;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Weapon _weapon;
        [SerializeField] private float _speed;
        [SerializeField] private List<Image> _hearts;
        
        private StateMachine _stateMachine;
        private CompositeDisposable _disposable;
        private PlayerInput _playerInput;
        private Wallet _wallet;

        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Weapon Weapon => _weapon;
        public Wallet Wallet => _wallet;
        public float Speed => _speed;
            
        public IdleState IdleState { get; private set; }
        public MovingState MovingState { get; private set; }
        public AttackState AttackState { get; private set; }
        public KilledState KilledState { get; private set; }
        
        [Inject]
        public void Construct(PlayerInput playerInput, Wallet wallet)
        {
            _playerInput = playerInput;
            _wallet = wallet;
        }

        private void OnEnable()
        {
            _playerInput.Player.Attack.canceled += Attack;
        }

        private void OnDisable()
        {
            _playerInput.Player.Attack.canceled -= Attack;
        }

        private void Start()
        {
            InitializeStates();

            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            Observable.EveryUpdate()
                .Subscribe(_ => OnUpdate())
                .AddTo(_disposable);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == 6)
            {
                Image image = _hearts.LastOrDefault(heart => heart.gameObject.activeSelf);
                
                if (image != null) 
                    image.gameObject.SetActive(false);
                
                image = _hearts.LastOrDefault(heart => heart.gameObject.activeSelf);

                if (image == null)
                {
                    GameOver();
                }
            }
        }

        public void ChangeState(UnitStateBase unit)
        {
            _stateMachine.ChangeState(unit);
        }

        private void OnUpdate()
        {
            if (_stateMachine.CurrentState == null)
            {
                ChangeState(IdleState);
            }
            
            _stateMachine?.OnUpdate();
        }

        private void InitializeStates()
        {
            _stateMachine = new StateMachine();
            
            IdleState = new IdleState(this, _playerInput);
            MovingState = new MovingState(this, _playerInput);
            AttackState = new AttackState(this, _playerInput, Weapon);
            KilledState = new KilledState(this);

            _stateMachine.ChangeState(IdleState);
        }

        private void Attack(InputAction.CallbackContext obj)
        {
            ChangeState(AttackState);
        }

        private void GameOver()
        {
            SceneManager.LoadScene("Tavern");
        }
    }
}