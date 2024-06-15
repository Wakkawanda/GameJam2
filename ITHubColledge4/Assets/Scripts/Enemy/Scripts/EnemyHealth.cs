using Scripts;
using UnityEngine;
using Zenject;

namespace Enemy.Scripts
{
    public class EnemyHealth : MonoBehaviour
    {
        private Wallet _wallet;
        
        [Inject]
        public void Construct(Wallet wallet)
        {
            _wallet = wallet;
        }
        
        public void TakeDamage(int damage)
        {
            Death();
        }

        private void Death()
        {
            _wallet.AddMoney(5);
            Destroy(gameObject);
        }
    }
}