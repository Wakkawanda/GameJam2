using System;
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
        [SerializeField] private Vector2 _attackSize = new Vector2(0.5f, 1f);
        [SerializeField] private int _damage = 10;

        public bool IsAttack { get; set; }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_attackPoint.position, _attackSize);

            Gizmos.DrawLine(_attackPoint.position, _attackPoint.position + new Vector3(-_attackSize.x / 2, 0, 0));
            Gizmos.DrawLine(_attackPoint.position, _attackPoint.position + new Vector3(_attackSize.x / 2, 0, 0));
            Gizmos.DrawLine(_attackPoint.position, _attackPoint.position + new Vector3(0, -_attackSize.y / 2, 0));
            Gizmos.DrawLine(_attackPoint.position, _attackPoint.position + new Vector3(0, _attackSize.y / 2, 0));
        }

        public async UniTaskVoid Attack()
        {
            transform.DOLocalRotate(_endAttackValue, 0.2f);
            transform.DOLocalMove(_endPositionAttackValue, 0.2f);
            Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(_attackPoint.position, _attackSize, 0, _enemyLayer);

            foreach (var enemy in enemiesHit)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(_damage);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            
            IsAttack = false;
            
            transform.DOLocalRotate(_startAttackValue, 0.2f);
            transform.DOLocalMove(_startPositionAttackValue, 0.2f);
        }
    }
}