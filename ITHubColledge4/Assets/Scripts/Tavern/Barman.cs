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
        [SerializeField] private List<Sprite> _images; // cutscene ones
        [SerializeField] private GameObject _imagesObject; // cutscene ones
        [SerializeField] private CanvasGroup _canvasGroup; // cutscene ones
        [SerializeField] private GameObject _canvas;
        [SerializeField] private CanvasGroup _endedCanvas;
        [SerializeField] private int _imageIndex = 0;
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _pricesText;
        [SerializeField] private TextMeshProUGUI _walletMoney;
        [SerializeField] private TextMeshProUGUI _newPricesText;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _skipButton;
        
        private Wallet _wallet;

        public static int Prices = 100;

        [Inject]
        public void Construct(Wallet wallet)
        {
            _wallet = wallet;
        }

        private void Awake()
        {
            _walletMoney.text = _wallet.GetMoneyValue().ToString();
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

            if (Prices > _wallet.GetMoneyValue())
            {
                _skipButton.gameObject.SetActive(true);
                _buyButton.gameObject.SetActive(false);
            }
            else
            {
                _skipButton.gameObject.SetActive(false);
                _buyButton.gameObject.SetActive(true);
            }

            if (_wallet.GetMoneyValue() < Prices && UnlockSpells.Second)
            {
                _exitButton.gameObject.SetActive(true);
            }
            else
            {
                _exitButton.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            GoThroughImages();
        }

        private void OnEnable()
        {
            _buyButton.onClick.AddListener(CheckIfEnough);
            _exitButton.onClick.AddListener(() => StartCoroutine(ToLobby()));
            _skipButton.onClick.AddListener(() => StartCoroutine(ToGame()));
        }

        private void OnDisable()
        {
            _buyButton.onClick.RemoveListener(CheckIfEnough);
            _exitButton.onClick.RemoveListener(() => StartCoroutine(ToLobby()));
            _skipButton.onClick.RemoveListener(() => StartCoroutine(ToGame()));
        }

        private void CheckIfEnough()
        {
            _buyButton.interactable = false;
            if (_wallet.GetMoneyValue() >= Prices)
            {
                _wallet.RemoveMoney(Prices); 
                Debug.Log("player animation");
            }

            if (!UnlockSpells.Three && UnlockSpells.Second && UnlockSpells.First)
            {
                UnlockSpells.Three = true;
                Prices = 250;
            }
            if (!UnlockSpells.Second && UnlockSpells.First)
            {
                UnlockSpells.Second = true;
                Prices = 200;
            }
            if (!UnlockSpells.First)
            {
                UnlockSpells.First = true;
                Prices = 150;
            }

            StartCoroutine(ToGame());
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

        private IEnumerator ToGame()
        {
            _skipButton.interactable = false;
            
            yield return new WaitForSeconds(1);
            
            SceneManager.LoadScene("Game");
        }

        private IEnumerator ToLobby()
        {
            _exitButton.interactable = false;
            _canvas.SetActive(false);
            
            while (_endedCanvas.alpha < 1)
            {
                _endedCanvas.alpha += 0.01f;

                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(5);

            SceneManager.LoadScene("Lobby");
        }
    }
}