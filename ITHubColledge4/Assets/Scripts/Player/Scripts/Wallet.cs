using UnityEngine;

namespace Scripts
{
    public class Wallet
    {
        private const string Money = "Money";
        
        public void AddMoney(int value)
        {
            PlayerPrefs.SetInt(Money, PlayerPrefs.GetInt(Money) + value);
            PlayerPrefs.Save();
        }

        public void RemoveMoney(int value)
        {
            PlayerPrefs.SetInt(Money, PlayerPrefs.GetInt(Money) - value);
            PlayerPrefs.Save();
        }
    }
}