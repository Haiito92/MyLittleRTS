using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RTSSelector.Scripts.Runtime
{
    public class RTSSelector : MonoBehaviour
    {
        //Fields//
        //Selectables
        private List<RTSSelectable> _allRtsSelectables;
        private List<RTSSelectable> _currentSelection;
        
        //Selector Box
        private Vector2 _mouseStartPos;
        private Vector2 _mouseEndPos;
        [SerializeField] private RectTransform _selectorBox;
        [SerializeField] private Canvas _canvas;
        private bool _isSelecting = false;
        
        //Actions//
        public UnityAction OnSelectionStart;
        public UnityAction OnSelectionEnd;
        
        //UnityEvent//
        [SerializeField] private UnityEvent OnSelectionStartEvent;
        [SerializeField] private UnityEvent OnSelectionEndEvent;


        #region Properties
        public List<RTSSelectable> AllRtsSelectables => _allRtsSelectables;
        public bool IsSelecting => _isSelecting;
        #endregion
        
        #region Singleton

        private static RTSSelector _instance;
        public static RTSSelector Instance => _instance;

        #endregion
        
        private void Awake()
        {
            //Setup Singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
            
            //Setup Lists
            _allRtsSelectables = new List<RTSSelectable>();
            _currentSelection = new List<RTSSelectable>();
            
            //Subscribe UnityActions Invoke to their corresponding UnityEvent
            OnSelectionStartEvent.AddListener(() => OnSelectionStart?.Invoke());
            OnSelectionEndEvent.AddListener(() => OnSelectionEnd?.Invoke());
        }

        public void StartSelection(Vector2 mouseStartPos)
        {
            if (_currentSelection != null)
            {
                _currentSelection.ForEach(rtsSelectable => rtsSelectable.Unselect());
                _currentSelection.Clear();
            }
            
            _isSelecting = true;
            
            _mouseStartPos = mouseStartPos;
            _selectorBox.position = _mouseStartPos;
            
            _selectorBox.gameObject.SetActive(true);
            
            OnSelectionStartEvent.Invoke();
        }

        public void UpdateSelection(Vector2 currentMousePos)
        {
            UpdateSelectorBox(currentMousePos);
        }
        
        public List<RTSSelectable> FinishSelection()
        {
            _isSelecting = false;
            OnSelectionEndEvent.Invoke();
            
            _currentSelection =
                _allRtsSelectables.FindAll(rtsSelectable => _selectorBox.rect.Overlaps(rtsSelectable.GetScreenRect(),true));
            Debug.Log(_currentSelection.Count);
            _currentSelection.ForEach(rtsSelectable => rtsSelectable.Select());
            
            _selectorBox.gameObject.SetActive(false);
            
            return _currentSelection;
        }

        private void UpdateSelectorBox(Vector2 mouseCurrentPos)
        {
            _mouseEndPos = mouseCurrentPos;

            float width = _mouseEndPos.x - _mouseStartPos.x;
            float height = _mouseEndPos.y - _mouseStartPos.y;

            _selectorBox.position = _mouseStartPos + new Vector2(width / 2, height / 2);
            _selectorBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(Camera.main.ScreenToWorldPoint(_selectorBox.rect.center), 0.5f);
        }
    }
}
