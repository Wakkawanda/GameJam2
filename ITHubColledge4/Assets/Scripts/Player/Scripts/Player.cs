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
using weed;
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
        [SerializeField] private float _attackVortexSize = 2;
        [SerializeField] private float _canCooldownAttack = 0.5f;
        [SerializeField] private float _canCooldownAttackJumpFrantic = 5;
        [SerializeField] private float _canCooldownAttackSmoke = 7;
        [SerializeField] private float _canCooldownAttackVortex = 10;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Image _jumpFranticView;
        [SerializeField] private Image _smokeView;
        [SerializeField] private Image _vortexView;
        [SerializeField] private Image _jumpFranticViewLock;
        [SerializeField] private Image _smokeViewLock;
        [SerializeField] private Image _vortexViewLock;
        [SerializeField] private AudioSource _miss;
        public float hurtTimer;
        public bool isHurt = false;
        public float radiusCheck = 3f;
        public LayerMask enemyLayer;

        private StateMachine _stateMachine;
        private CompositeDisposable _disposable;
        private PlayerInput _playerInput;
        private Wallet _wallet;
        private bool _isVortex;
        private float _currentTimeAttack;
        private float _currentTimeJumpFrantic;
        private float _currentTimeSmoke;
        private float _currentTimeVortex;
        private bool _canAttack;
        private bool _canAttackJumpFrantic;
        private bool _canAttackSmoke;
        private bool _canAttackVortex;

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

            if (UnlockSpells.First)
            {
                _playerInput.Player.JumpFrantic.canceled += JumpFrantic;
                _jumpFranticViewLock.gameObject.SetActive(false);
            }
            else
            {
                _jumpFranticViewLock.gameObject.SetActive(true);
            }
            
            if (UnlockSpells.Second)
            {
                _playerInput.Player.Smoke.canceled += Smoke;
                _smokeViewLock.gameObject.SetActive(false);
            }
            else
            {
                _smokeViewLock.gameObject.SetActive(true);
            }
            
            if (UnlockSpells.Three)
            {
                _playerInput.Player.Vortex.canceled += Vortex;
                _vortexViewLock.gameObject.SetActive(false);
            }
            else
            {
                _vortexViewLock.gameObject.SetActive(true);
            }
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

            _currentTimeSmoke = _canCooldownAttackSmoke / 2;
            _currentTimeVortex = _canCooldownAttackVortex / 2;

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
            float timer = 0;
            
            while (timer < hurtTimer)
            {
                _spriteRenderer.color = new Color32(255, 255, 255, 255);
                
                yield return new WaitForSeconds(hurtTimer / 8);
                
                _spriteRenderer.color = new Color32(255, 255, 255, 145);
                
                yield return new WaitForSeconds(hurtTimer / 8);

                timer += hurtTimer / 4;
            }
            
            isHurt = false;
            _spriteRenderer.color = new Color32(255, 255, 255, 255);

            // this is a small check if player is hugging enemies
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

            if (!_canAttack)
            {
                _currentTimeAttack += Time.deltaTime;
                if (_currentTimeAttack > _canCooldownAttack)
                {
                    _canAttack = true;
                    _currentTimeAttack = 0;
                }
            }
            
            if (!_canAttackJumpFrantic)
            {
                _currentTimeJumpFrantic += Time.deltaTime;
                if (_currentTimeJumpFrantic > _canCooldownAttackJumpFrantic)
                {
                    _canAttackJumpFrantic = true;
                    _currentTimeJumpFrantic = 0;
                }
            }
            
            if (!_canAttackSmoke)
            {
                _currentTimeSmoke += Time.deltaTime;
                if (_currentTimeSmoke > _canCooldownAttackSmoke)
                {
                    _canAttackSmoke = true;
                    _currentTimeSmoke = 0;
                }
            }
            
            if (!_canAttackVortex)
            {
                _currentTimeVortex += Time.deltaTime;
                if (_currentTimeVortex > _canCooldownAttackVortex)
                {
                    _canAttackVortex = true;
                    _currentTimeVortex = 0;
                }
            }

            if(UnlockSpells.First)
                _jumpFranticView.fillAmount = _currentTimeJumpFrantic / _canCooldownAttackJumpFrantic;
            else
                _jumpFranticView.fillAmount = 1;
            
            if(UnlockSpells.Second)
                _smokeView.fillAmount = _currentTimeSmoke / _canCooldownAttackSmoke;
            else
                _smokeView.fillAmount = 1;
            
            if(UnlockSpells.Three)
                _vortexView.fillAmount = _currentTimeVortex / _canCooldownAttackVortex;
            else
                _vortexView.fillAmount = 1;
            
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
            if (_canAttack)
            {
                ChangeState(AttackState);
                _miss.Play();
                _canAttack = false;
            }
        }

        private void JumpFrantic(InputAction.CallbackContext obj)
        {
            if (_canAttackJumpFrantic)
            {
                ChangeState(JumpFranticState);
                _canAttackJumpFrantic = false;
            }
        }
        
        private void Smoke(InputAction.CallbackContext obj)
        {
            if (_canAttackSmoke)
            {
                ChangeState(SmokeState);
                _canAttackSmoke = false;
            }
        }
       
        private void Vortex(InputAction.CallbackContext obj)
        {
            if (_canAttackVortex)
            {
                _animator.SetTrigger("Vortex");
                Weapon.gameObject.SetActive(false);
                _canAttackVortex = false;
            }
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

        public void AttackVortex()
        {
            _isVortex = true;
            StartCoroutine(OnAttackVortex());
        }
        
        private IEnumerator OnAttackVortex()
        {
            Debug.Log("Attack JumpFrantic");

            while (_isVortex)
            {
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(_attackPoint.position, _attackVortexSize, _enemyLayer);

                foreach (var enemy in enemiesHit)
                {
                    if (enemy.TryGetComponent(out EnemyTemplate health))
                    {
                        health.TakeDamage(0);
                    }
                }

                yield return null;
            }
        }
        
        public void StopVortex()
        {
            _isVortex = false;
            Weapon.gameObject.SetActive(true);
            ChangeState(MovingState);
            Debug.Log("Stop JumpFrantic");
        }
    }
}