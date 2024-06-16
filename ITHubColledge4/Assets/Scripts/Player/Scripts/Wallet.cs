using UnityEngine;

namespace Scripts
{
    public class Wallet
    {
        private const string Money = "Money";
        private const string FirstCutscene = "FirstCutscene";

        public int GetMoneyValue()
        {
            return PlayerPrefs.GetInt(Money);
        }

        public int GetFirstCutscene()
        {
            return PlayerPrefs.GetInt(FirstCutscene);
        }

        public void SetFirstCutscene(int status)
        {
            PlayerPrefs.SetInt(FirstCutscene, status > 0 ? 1 : 0);
            PlayerPrefs.Save();
        }

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