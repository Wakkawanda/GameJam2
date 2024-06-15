using Scripts;
using TMPro;
using UnityEngine;
using Zenject;

namespace WalletLogic
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _money;
        private Wallet _wallet;
        
        [Inject]
        public void Construct(Wallet wallet)
        {
            _wallet = wallet;
        }

        private void Update()
        {
            _money.text = $"{_wallet.GetMoneyValue()}";
        }
    }
}