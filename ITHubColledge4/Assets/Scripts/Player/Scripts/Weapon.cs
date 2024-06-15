using Enemy.Scripts;
using UnityEngine;

namespace Scripts
{
    public class Weapon : MonoBehaviour
    {
        public Transform attackPoint;
        public LayerMask enemyLayer;
        public Vector2 attackSize = new Vector2(0.5f, 1f); // Width and height of the rectangle
        public int damage = 10;

        public bool IsAttack { get; set; }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(attackPoint.position, attackSize);

            // Optionally draw lines to visualize the extent of the attack
            Gizmos.DrawLine(attackPoint.position, attackPoint.position + new Vector3(-attackSize.x / 2, 0, 0));
            Gizmos.DrawLine(attackPoint.position, attackPoint.position + new Vector3(attackSize.x / 2, 0, 0));
            Gizmos.DrawLine(attackPoint.position, attackPoint.position + new Vector3(0, -attackSize.y / 2, 0));
            Gizmos.DrawLine(attackPoint.position, attackPoint.position + new Vector3(0, attackSize.y / 2, 0));
        }

        public void Attack()
        {
            Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0, enemyLayer);

            foreach (var enemy in enemiesHit)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }

            IsAttack = false;
        }
    }
}