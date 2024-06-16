using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ShakerText
{
    public class ShakerText : MonoBehaviour
    {
        [SerializeField] private List<Image> _textImage;
        [SerializeField] private float _beatInterval = 0.5f;

        private float _timer;
        private Tweener _tweener;

        private void Start()
        {
            _timer = _beatInterval;
        }

        private void OnDisable()
        {
            foreach (Image image in _textImage)
            {
                image.transform.DOKill();
            }
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _beatInterval)
            {
                ShakeText().Forget();
                _timer = 0;
            }
        }

        private async UniTaskVoid ShakeText()
        {
            foreach (Image image in _textImage)
            {
                Vector3 scale = image.transform.localScale;
                await image.transform.DOScale(scale * 1.2f, 0.2f);
                await image.transform.DOScale(scale, 0.1f);
            }
        }
    }
}