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
        [SerializeField] private List<Sprite> _images; // cutscene ones
        [SerializeField] private GameObject _imagesObject; // cutscene ones
        [SerializeField] private CanvasGroup _canvasGroup; // cutscene ones
        [SerializeField] private int _imageIndex = 0;
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

        private void Start()
        {
            GoThroughImages();
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

        private void GoThroughImages() 
        {
            // StartCoroutine(SwitchImageOnInput());
            StartCoroutine("SwitchImageAuto");
            // fade in/out
        }

        private IEnumerator SwitchImageAuto() 
        {
            foreach (Sprite sprite in _images)
            {
                // show what we got
                FadeIn(_canvasGroup, 1f);
                yield return new WaitForSeconds(2f);
                
                // hide what we got
                FadeOut(_canvasGroup, 1f);
                yield return new WaitForSeconds(2f);
                
                // get next what we got
                _imagesObject.GetComponent<Image>().sprite = _images[_imageIndex];
                _imageIndex = (_imageIndex + 1) % _images.Count;
            }
            StartCoroutine("SwitchImageAuto");  // loop
        }

        private IEnumerator Fade(CanvasGroup c, float startAlpha, float endAlpha, float time)
        {
            float startTime = Time.time;

            while (Time.time - startTime < time)
            {
                float t = (Time.time - startTime) / time;
                t = t * t * (3f - 2f * t); // smoothstep function
                float currentAlpha = Mathf.MoveTowards(startAlpha, endAlpha, t);
                c.alpha = currentAlpha;
                yield return null;
            }

            c.alpha = endAlpha;
        }

        public void FadeIn(CanvasGroup c, float time)
        {
            StartCoroutine(Fade(c, 0f, 1f, time));
        }

        public void FadeOut(CanvasGroup c, float time)
        {
            StartCoroutine(Fade(c, 1f, 0f, time));
        }

        private IEnumerator SwitchImageOnInput() {  // todo?
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            _imagesObject.GetComponent<Image>().sprite = _images[_imageIndex];
            _imageIndex = (_imageIndex + 1) % _images.Count;
        }

        private IEnumerator WaitForInputAndSendToHell()
        {
            yield return new WaitForSeconds(1);
            
            SceneManager.LoadScene("Game");
        }
    }
}