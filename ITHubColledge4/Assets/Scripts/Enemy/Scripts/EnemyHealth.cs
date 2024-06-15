using UnityEngine;

namespace Enemy.Scripts
{
    public class EnemyHealth : MonoBehaviour
    {
        public void TakeDamage(int damage)
        {
            Death();
        }

        private void Death()
        {
            Destroy(gameObject);
        }
    }
}