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
        [SerializeField] private List<Sprite> _imagesFirstCutscene; // cutscene ones
        [SerializeField] private List<Sprite> _imagesRepeatingCutscene; // cutscene ones
        [SerializeField] private GameObject _imagesObject; // cutscene ones
        [SerializeField] private CanvasGroup _canvasGroup; // cutscene ones
        [SerializeField] private GameObject _canvas;
        [SerializeField] private CanvasGroup _endedCanvas;
        [SerializeField] private int _imageIndex = 0;
        [SerializeField] private int _imageIndexToStopAt = 0;
        [SerializeField] private TextMeshProUGUI _pricesText;
        [SerializeField] private TextMeshProUGUI _walletMoney;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private CanvasGroup _buyButtonCanvas;
        [SerializeField] private CanvasGroup _skipButtonCanvas;
        [SerializeField] private CanvasGroup _exitButtonCanvas;

        private Wallet _wallet;
        public static int Prices = 50;
        public static bool First = true;

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
            }
            
            if (UnlockSpells.Three)
            {
                _pricesText.text = "Нет в наличии";
            }
            else
            {
                _pricesText.text = $"{Prices}";
            }
        }

        private void Start()
        {
            GoThroughImages(First);
        }

        private void OnEnable()
        {
            _buyButton.onClick.AddListener(() => StartCoroutine(CheckIfEnough()));
            _exitButton.onClick.AddListener(() => StartCoroutine(ToLobby()));
            _skipButton.onClick.AddListener(() => StartCoroutine(ToGame()));
        }

        private void OnDisable()
        {
            _buyButton.onClick.RemoveListener(() => StartCoroutine(CheckIfEnough()));
            _exitButton.onClick.RemoveListener(() => StartCoroutine(ToLobby()));
            _skipButton.onClick.RemoveListener(() => StartCoroutine(ToGame()));
        }

        private IEnumerator CheckIfEnough()
        {
            _buyButtonCanvas.alpha = 0;
            _buyButton.interactable = false;
            _buyButtonCanvas.blocksRaycasts = false;
            _buyButtonCanvas.interactable = false;
            _pricesText.gameObject.SetActive(false);
            
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
                Prices = 150;
            }
            if (!UnlockSpells.First)
            {
                UnlockSpells.First = true;
                Prices = 100;
            }
            
            _imagesObject.GetComponent<Image>().sprite = _imagesRepeatingCutscene[_imagesRepeatingCutscene.Count - 2];
            FadeIn(_canvasGroup, 0.7f);
            yield return new WaitForSeconds(2f);
            
            _imagesObject.GetComponent<Image>().sprite = _imagesRepeatingCutscene[_imagesRepeatingCutscene.Count - 1];
            FadeIn(_canvasGroup, 0.7f);
            yield return new WaitForSeconds(2f);

            while (_skipButtonCanvas.alpha < 1)
            {
                _skipButtonCanvas.alpha += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            _skipButtonCanvas.interactable = true;
            _skipButtonCanvas.blocksRaycasts = true;
        }

        private void GoThroughImages(bool first) 
        {
            // StartCoroutine(SwitchImageOnInput());
            StartCoroutine("SwitchImageAuto", first);
            // fade in/out

            // todo play until the necessary one
            // pause at that; after work/??? continue playing
            // when last is reached we go to game
        }

        private void EnableUI() 
        {
            // code here that will show us the magazine ui...
        }

        private void DisableUI() 
        {
            // the opposite of enable UI...
            // trigger this when shopping is done,
            // and after this also trigger switchImageAuto again

        }

        private IEnumerator SwitchImageAuto(bool first) 
        {
            var currentImages = first ? _imagesFirstCutscene : _imagesRepeatingCutscene;
            First = false;
            
            foreach (Sprite sprite in currentImages)
            {                
                // get next what we got
                _imagesObject.GetComponent<Image>().sprite = currentImages[_imageIndex];
                _imageIndex = (_imageIndex + 1);

                // if we get too much go to game
                if (_imageIndex > currentImages.Count)
                {
                    //StartCoroutine(ToGame());
                }
                
                // show what we got
                FadeIn(_canvasGroup, 0.7f);
                
                if (UnlockSpells.Three && _imageIndex == 3)
                {
                    yield return new WaitForSeconds(2.5f);

                }
                else
                {
                    yield return new WaitForSeconds(1.5f);
                }
                
                if (_imageIndex >= currentImages.Count)
                {
                    break;
                }

                if (_imageIndex == 6 || _imageIndex == 3 && !first)
                {
                    if (_wallet.GetMoneyValue() >= Prices)
                    {
                        while (_buyButtonCanvas.alpha < 1)
                        {
                            _buyButtonCanvas.alpha += 0.01f;
                            yield return new WaitForSeconds(0.01f);
                        }
                        
                        _buyButtonCanvas.blocksRaycasts = true;
                        _buyButtonCanvas.interactable = true;
                        _buyButton.interactable = true;
                        
                        break;
                    }
                }
                else
                {
                    if (_wallet.GetMoneyValue() < Prices)
                    {
                        _buyButtonCanvas.alpha = 0;
                        _buyButtonCanvas.blocksRaycasts = false;
                        _buyButtonCanvas.interactable = false;
                    }
                }
                
                // if this is our stop we stop
                if (!first && _imageIndex == _imageIndexToStopAt) yield break;

                // hide what we got
                //FadeOut(_canvasGroup, 0.5f);
                
                if (_imageIndex == 5 || _imageIndex == 2 && !first)
                {
                    yield return new WaitForSeconds(0.5f);
                    
                    _pricesText.gameObject.SetActive(true);
                }
                else
                {
                    _pricesText.gameObject.SetActive(false);
                }
                
                //yield return new WaitForSeconds(1.5f);
            }
            //StartCoroutine("SwitchImageAuto", false);  // loop
            
            if (Prices > _wallet.GetMoneyValue())
            {
                _buyButtonCanvas.alpha = 0; 
                _buyButtonCanvas.blocksRaycasts = false; 
                _buyButtonCanvas.interactable = false; 

                while (_skipButtonCanvas.alpha < 1)
                {
                    _skipButtonCanvas.alpha += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }

                _skipButtonCanvas.interactable = true;
                _skipButtonCanvas.blocksRaycasts = true;
            }
            else
            {
                _skipButtonCanvas.alpha = 0; 
                _skipButtonCanvas.blocksRaycasts = false; 
                _skipButtonCanvas.interactable = false; 

                while (_buyButtonCanvas.alpha < 1)
                {
                    _buyButtonCanvas.alpha += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }

                _buyButtonCanvas.interactable = true;
                _buyButtonCanvas.blocksRaycasts = true;
            }
            
            if (_wallet.GetMoneyValue() < Prices && UnlockSpells.Second)
            {
                while (_exitButtonCanvas.alpha < 1)
                {
                    _exitButtonCanvas.alpha += 0.01f;

                    yield return new WaitForSeconds(0.01f);
                }

                _exitButtonCanvas.blocksRaycasts = true;
                _exitButtonCanvas.interactable = true;
            }
            else
            {
                _exitButtonCanvas.alpha = 0f;
                _exitButtonCanvas.blocksRaycasts = false;
                _exitButtonCanvas.interactable = false;
            }
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
            // _imagesObject.GetComponent<Image>().sprite = _images[_imageIndex];
            // _imageIndex = (_imageIndex + 1) % _images.Count;
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