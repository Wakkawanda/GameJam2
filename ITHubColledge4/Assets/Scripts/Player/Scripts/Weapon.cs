using Enemy.Scripts;
using UnityEngine;

namespace Scripts
{
    public class Weapon : MonoBehaviour
    {
        public Transform attackPoint;
        public LayerMask enemyLayer;
        public float attackRange = 0.5f;
        public int damage = 10;
        
        public bool IsAttack { get; set; }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);

            Gizmos.DrawLine(attackPoint.position, attackPoint.position + new Vector3(-attackRange * 2, 0, 0));
            Gizmos.DrawLine(attackPoint.position, attackPoint.position + new Vector3(attackRange * 2, 0, 0));
        }
        
        public void Attack()
        {
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            
            foreach (var enemy in enemiesHit)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            
            IsAttack = false;
        }
    }
}