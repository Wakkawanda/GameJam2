using Febucci.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tavern
{
    public class ButtonGrass : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextAnimator _text;

        private void Start()
        {
            _text.effectIntensityMultiplier = 0;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _text.effectIntensityMultiplier = 50;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _text.effectIntensityMultiplier = 0;
        }
    }
}