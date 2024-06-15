using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemy.Scripts;
using UnityEngine;

namespace Scripts
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Vector3 _startAttackValue;
        [SerializeField] private Vector3 _startPositionAttackValue;
        [SerializeField] private Vector3 _endAttackValue;
        [SerializeField] private Vector3 _endPositionAttackValue;
        [SerializeField] private Player _player;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _attackSize = 5;
        [SerializeField] private int _damage = 10;

        public bool IsAttack { get; set; }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackSize);
            
            Gizmos.DrawLine(_attackPoint.position, _attackPoint.position + new Vector3(-_attackSize * 2, 0, 0));
            Gizmos.DrawLine(_attackPoint.position, _attackPoint.position + new Vector3(_attackSize * 2, 0, 0));
        }

        public async UniTaskVoid Attack()
        {
            float timer = 0;
            float targetTime = 0.2f;
            
            transform.DOLocalRotate(_endAttackValue, targetTime);
            transform.DOLocalMove(_endPositionAttackValue, targetTime);

            while (timer < targetTime)
            {
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(_attackPoint.position, _attackSize, _enemyLayer);

                foreach (var enemy in enemiesHit)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(_damage);
                }

                timer += Time.deltaTime;

                await UniTask.Yield();
            }
            
            IsAttack = false;
            
            transform.DOLocalRotate(_startAttackValue, targetTime);
            transform.DOLocalMove(_startPositionAttackValue, targetTime);
        }
    }
}