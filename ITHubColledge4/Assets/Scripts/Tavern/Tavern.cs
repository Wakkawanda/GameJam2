using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace weed
{
    public class Barman : MonoBehaviour
    {
        [SerializeField] private List<int> _prices;
        private Wallet _wallet;

        public void Construct(Wallet wallet)
        {
            _wallet = wallet;
        }

        public void CheckIfEnough()
        {
            if (_wallet.GetMoneyValue() >= _prices[0]) //todo change counter
            {
                _wallet.RemoveMoney(_prices[0]); 
                Debug.Log("player animation"); //todo event for player
            }

            StartCoroutine(WaitForInputAndSendToHell());
        }

        public IEnumerator WaitForInputAndSendToHell()
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            
            SceneManager.LoadScene("Game");
        }
    }
}