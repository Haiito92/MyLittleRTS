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
            #region Setup Singleton
            //Setup Singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
            #endregion
            
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
            
            _mouseStartPos = mouseStartPos * 1/_canvas.scaleFactor;
            
            _selectorBox.anchoredPosition = _mouseStartPos;
            _selectorBox.sizeDelta = new Vector2(0f, 0f);
            
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

            Vector3[] corners = new Vector3[4];
            _selectorBox.GetWorldCorners(corners);

            Vector2 selectorMin = corners[0];
            Vector2 selectorMax = corners[2];

            foreach (RTSSelectable rtsSelectable in _allRtsSelectables)
            {
                Vector3[] selectableCorners = new Vector3[4];
                rtsSelectable.RectTransform.GetWorldCorners(selectableCorners);
                Vector2 selectableMin = selectableCorners[0];
                Vector2 selectableMax = selectableCorners[2];

                if (selectorMin.x < selectableMax.x &&
                    selectorMax.x > selectableMin.x &&
                    selectorMin.y < selectableMax.y &&
                    selectorMax.y > selectableMin.y)
                {
                    _currentSelection.Add(rtsSelectable);
                }
            }
            _currentSelection.ForEach(rtsSelectable => rtsSelectable.Select());
            
            _selectorBox.gameObject.SetActive(false);
            
            return _currentSelection;
        }

        private void UpdateSelectorBox(Vector2 mouseCurrentPos)
        {
            _mouseEndPos = mouseCurrentPos * 1/_canvas.scaleFactor;

            float width = _mouseEndPos.x - _mouseStartPos.x;
            float height = _mouseEndPos.y - _mouseStartPos.y;
            
            _selectorBox.anchoredPosition = _mouseStartPos + new Vector2(width / 2, height / 2);
            _selectorBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        }

        private void OnDrawGizmos()
        {
            // Gizmos.DrawWireSphere(Camera.main.ScreenToWorldPoint(_selectorBox.rect.center), 0.5f);
        }
    }
}
