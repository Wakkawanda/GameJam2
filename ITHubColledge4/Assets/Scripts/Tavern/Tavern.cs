using System.Collections;
using System.Collections.Generic;
using Scripts;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _pricesText;
        [SerializeField] private TextMeshProUGUI _newPricesText;
        private Wallet _wallet;

        public static int Prices = 100;

        [Inject]
        public void Construct(Wallet wallet)
        {
            _wallet = wallet;
        }

        private void Awake()
        {
            if (_wallet.GetMoneyValue() > Prices)
            {
                if (UnlockSpells.Three)
                {
                    Prices = _wallet.GetMoneyValue() + 1;
                }
                else
                {
                    Prices = _wallet.GetMoneyValue();
                }
                
                _newPricesText.gameObject.SetActive(true);
            }
            else
            {
                _newPricesText.gameObject.SetActive(false);
            }
            
            _pricesText.text = $"{Prices}";
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
            if (_wallet.GetMoneyValue() >= Prices)
            {
                _wallet.RemoveMoney(Prices); 
                Debug.Log("player animation");
            }

            if (!UnlockSpells.Three && UnlockSpells.Second && UnlockSpells.First)
            {
                UnlockSpells.Three = true;
                Prices = 600;
            }
            if (!UnlockSpells.Second && UnlockSpells.First)
            {
                UnlockSpells.Second = true;
                Prices = 400;
            }
            if (!UnlockSpells.First)
            {
                UnlockSpells.First = true;
                Prices = 200;
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