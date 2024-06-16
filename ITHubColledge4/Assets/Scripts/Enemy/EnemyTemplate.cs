using Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Enemy
{
    public class EnemyTemplate : MonoBehaviour
    {
        [SerializeField] private int attack;
        [SerializeField] private int moneyDrop;
        [SerializeField] private Player player;
        private GameObject playerObject;
        [SerializeField] private NavMeshAgent agent;
        // enemy type here maybe? i.e. melee or long-range or whatever
        // pick attack from enemy type 

        private Wallet _wallet;

        [Inject]
        public void Construct(Wallet wallet)
        {
            _wallet = wallet;
        }

        private readonly string gotoplayerfuncname = "GoToPlayer";
        private readonly int gotoplayerfunctimeout = 5;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (player == null) {
                string plr = "Player";
                playerObject = GameObject.Find(plr);
                player = playerObject.GetComponent<Player>(); // its gamejab
            }

            _wallet = player.Wallet;
            //enemyHealth = new();
            //enemyHealth.die.AddListener(Destroy);
        }

        // Update is called once per frame
        void Update()
        {
            // if (enemyHealth.isDead) return;
            StartCoroutine(gotoplayerfuncname);
        }

        public void TakeDamage(int damage)
        {
            Debug.Log("Take damage");
            Death();
        }

        private void Death()
        {
            _wallet.AddMoney(Random.Range(5, 20));
            Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }
        
        private IEnumerator GoToPlayer()
        {
            agent.destination = player.transform.position;
            yield return new WaitForSeconds(gotoplayerfunctimeout);
        }

        // todo try to attack player

        // todo distance from player

    }
}
