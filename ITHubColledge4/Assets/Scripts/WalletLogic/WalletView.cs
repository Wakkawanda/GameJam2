using Scripts;
using TMPro;
using UnityEngine;
using weed;
using Zenject;

namespace WalletLogic
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _money;
        [SerializeField] private TextMeshProUGUI _moneyTarget;
        private Wallet _wallet;
        
        [Inject]
        public void Construct(Wallet wallet)
        {
            _wallet = wallet;
        }

        private void Start()
        {
            if (UnlockSpells.Three)
            {
                _moneyTarget.text = "???";
            }
            else
            {
                _moneyTarget.text = $"{Barman.Prices}";
            }
        }

        private void Update()
        {
            _money.text = $"{_wallet.GetMoneyValue()}";
        }
    }
}