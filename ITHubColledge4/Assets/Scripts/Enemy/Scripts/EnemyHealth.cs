using UnityEngine;

namespace Enemy.Scripts
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int _health = 100;
        
        public void TakeDamage(int damage)
        {
            Debug.Log("Take damage");
            _health -= damage;
            
            if (_health <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            Destroy(gameObject);
        }
    }
}