using RTSSelector.Scripts.Runtime.Core;
using UnityEngine;

namespace RTSSelector.Scripts.Runtime.OOP.UI
{
    public class RTSSelectorBox : MonoBehaviour
    {
        private OOP.Core.RTSSelector _rtsSelector;

        [SerializeField] private RectTransform _selectorBox;
        [SerializeField] private Canvas _canvas;
        
        private void Start()
        {
            _rtsSelector = OOP.Core.RTSSelector.Instance;
            
            _rtsSelector.SelectionStarted += OnSelectionStarted;
            _rtsSelector.SelectionUpdated += OnSelectionUpdated;
            _rtsSelector.SelectionEnded += OnSelectionEnded;
        }

        private void OnSelectionStarted()
        {
            _selectorBox.gameObject.SetActive(true);
            _selectorBox.anchoredPosition = _rtsSelector.SelectorRect.Center * 1 / _canvas.scaleFactor;
            _selectorBox.sizeDelta = new Vector2(0f, 0f);
        }
        
        private void OnSelectionUpdated()
        {
            RTSScreenRect selectorRect = _rtsSelector.SelectorRect;
            _selectorBox.anchoredPosition = selectorRect.Center * 1 / _canvas.scaleFactor;
            _selectorBox.sizeDelta =
                new Vector2(selectorRect.Width * 1 / _canvas.scaleFactor, selectorRect.Height * 1 / _canvas.scaleFactor);
        }
        
        private void OnSelectionEnded()
        {
            _selectorBox.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            
            if(_rtsSelector == null) return;
            _rtsSelector.SelectionStarted += OnSelectionStarted;
            _rtsSelector.SelectionUpdated += OnSelectionUpdated;
            _rtsSelector.SelectionEnded += OnSelectionEnded;
        }

        private void OnDisable()
        {
            if(_rtsSelector == null) return;
            _rtsSelector.SelectionStarted -= OnSelectionStarted;
            _rtsSelector.SelectionUpdated -= OnSelectionUpdated;
            _rtsSelector.SelectionEnded -= OnSelectionEnded;
        }
    }
}
