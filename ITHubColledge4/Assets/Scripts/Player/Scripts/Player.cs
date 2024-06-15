using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Enemy;
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
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speed;
        [SerializeField] private List<Image> _hearts;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _attackSize = 5;
        public float hurtTimer;
        public bool isHurt = false;
        public float radiusCheck = 3f;
        public LayerMask enemyLayer;

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
        public JumpFranticState JumpFranticState { get; private set; }
        public SmokeState SmokeState { get; private set; }
        public VortexState VortexState { get; private set; }
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
            _playerInput.Player.JumpFrantic.canceled += JumpFrantic;
            _playerInput.Player.Smoke.canceled += Smoke;
            _playerInput.Player.Vortex.canceled += Vortex;
        }

        private void OnDisable()
        {
            _playerInput.Player.Attack.canceled -= Attack;
            _playerInput.Player.JumpFrantic.canceled -= JumpFrantic;
            _playerInput.Player.Smoke.canceled -= Smoke;
            _playerInput.Player.Vortex.canceled -= Vortex;
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

        private void TakeDamage() {
            StartCoroutine("DamageTimer");
            Image image = _hearts.LastOrDefault(heart => heart.gameObject.activeSelf);
            
            if (image != null) 
                image.gameObject.SetActive(false);
            
            image = _hearts.LastOrDefault(heart => heart.gameObject.activeSelf);

            if (image == null)
            {
                GameOver();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == 6 && !isHurt)
            {
                TakeDamage();
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackSize);
            
            Gizmos.DrawLine(_attackPoint.position, _attackPoint.position + new Vector3(-_attackSize * 2, 0, 0));
            Gizmos.DrawLine(_attackPoint.position, _attackPoint.position + new Vector3(_attackSize * 2, 0, 0));
        }

        public void ChangeState(UnitStateBase unit)
        {
            _stateMachine.ChangeState(unit);
        }

        public IEnumerator DamageTimer() 
        {
            isHurt = true;
            yield return new WaitForSeconds(hurtTimer);
            isHurt = false;
            Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position,
                radiusCheck, 
                enemyLayer);
            if (enemyColliders.Length > 0)
            { 
                TakeDamage();
            }
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
            
            IdleState = new IdleState(this, _playerInput, _animator);
            MovingState = new MovingState(this, _playerInput, _animator);
            AttackState = new AttackState(this, _playerInput, Weapon);
            JumpFranticState = new JumpFranticState(this, _playerInput, _animator);
            SmokeState = new SmokeState(this, _playerInput);
            VortexState = new VortexState(this, _playerInput);
            KilledState = new KilledState(this);

            _stateMachine.ChangeState(IdleState);
        }

        private void Attack(InputAction.CallbackContext obj)
        {
            ChangeState(AttackState);
        }

        private void JumpFrantic(InputAction.CallbackContext obj)
        {
            ChangeState(JumpFranticState);
        }
        
        private void Smoke(InputAction.CallbackContext obj)
        {
            ChangeState(SmokeState);
        }
       
        private void Vortex(InputAction.CallbackContext obj)
        {
            ChangeState(VortexState);
        }

        private void GameOver()
        {
            SceneManager.LoadScene("Tavern");
        }

        public void AttackJumpFrantic()
        {
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(_attackPoint.position, _attackSize, _enemyLayer);

            foreach (var enemy in enemiesHit)
            {
                if (enemy.TryGetComponent(out EnemyTemplate health))
                {
                    health.TakeDamage(0);
                }
            }
            Debug.Log("Attack JumpFrantic");
        }

        public void StopJumpFrantic()
        {
            Weapon.gameObject.SetActive(true);
            ChangeState(MovingState);
            Debug.Log("Stop JumpFrantic");
        }
    }
}