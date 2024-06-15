using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace weed
{
    public class Barman : MonoBehaviour
    {
        [SerializeField] private List<int> _prices;
        [SerializeField] private List<Button> _buttons;
        private Wallet _wallet;

        [Inject]
        public void Construct(Wallet wallet)
        {
            _wallet = wallet;
        }

        private void OnEnable()
        {
            foreach (Button button in _buttons)
            {
                button.onClick.AddListener(CheckIfEnough);
            }
        }

        private void OnDisable()
        {
            foreach (Button button in _buttons)
            {
                button.onClick.RemoveListener(CheckIfEnough);
            }
        }

        private void CheckIfEnough()
        {
            if (_wallet.GetMoneyValue() >= _prices[0]) //todo change counter
            {
                _wallet.RemoveMoney(_prices[0]); 
                Debug.Log("player animation"); //todo event for player
            }

            StartCoroutine(WaitForInputAndSendToHell());
        }

        private IEnumerator WaitForInputAndSendToHell()
        {
            yield return new WaitForSeconds(1);
            
            SceneManager.LoadScene("Game");
        }
    }
}