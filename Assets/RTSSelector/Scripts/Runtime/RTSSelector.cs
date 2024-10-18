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
        private Vector2 _scaledMouseStartPos;
        private Vector2 _scaledMouseEndPos;
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

        #region Selection
        public void StartSelection(Vector2 mouseStartPos)
        {
            if (_currentSelection != null)
            {
                _currentSelection.ForEach(rtsSelectable => rtsSelectable.Unselect());
                _currentSelection.Clear();
            }
            
            _isSelecting = true;
            
            _scaledMouseStartPos = mouseStartPos * 1/_canvas.scaleFactor;
            
            _selectorBox.anchoredPosition = _scaledMouseStartPos;
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

            //We get the min corner of rectTransform
            Vector3[] corners = new Vector3[4];
            _selectorBox.GetWorldCorners(corners);
            Vector2 selectorMin = corners[0];

            //We multiply by canvas.scaleFactor to cancel the previous multiply by 1/canvas.scaleFactor
            RTSScreenRect selectorRect =
                new RTSScreenRect(selectorMin, _selectorBox.rect.width * _canvas.scaleFactor, _selectorBox.rect.height * _canvas.scaleFactor);
            
            //iterate to see which selectable are in rectangle
            foreach (RTSSelectable rtsSelectable in _allRtsSelectables)
            {
                RTSScreenRect selectableRect = rtsSelectable.GetScreenRect();
                if (selectorRect.Overlaps(selectableRect)) _currentSelection.Add(rtsSelectable);
            }
            
            //Select all selectable in currentSelection
            _currentSelection.ForEach(rtsSelectable => rtsSelectable.Select());
            
            _selectorBox.gameObject.SetActive(false);
            
            return _currentSelection;
        }
        #endregion
        
        private void UpdateSelectorBox(Vector2 mouseCurrentPos)
        {
            _scaledMouseEndPos = mouseCurrentPos * 1/_canvas.scaleFactor;

            float width = _scaledMouseEndPos.x - _scaledMouseStartPos.x;
            float height = _scaledMouseEndPos.y - _scaledMouseStartPos.y;
            
            _selectorBox.anchoredPosition = _scaledMouseStartPos + new Vector2(width / 2, height / 2);
            _selectorBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        }

        #region Deprecated
        // private bool SelectorBoxOverlaps(RectTransform other)
        // {
        //     Vector3[] corners = new Vector3[4];
        //     _selectorBox.GetWorldCorners(corners);
        //     Vector2 selectorMin = corners[0];
        //     Vector2 selectorMax = corners[2];
        //     
        //     Vector3[] selectableCorners = new Vector3[4];
        //     other.GetWorldCorners(selectableCorners);
        //     Vector2 selectableMin = selectableCorners[0];
        //     Vector2 selectableMax = selectableCorners[2];
        //
        //     return selectorMin.x < selectableMax.x &&
        //            selectorMax.x > selectableMin.x &&
        //            selectorMin.y < selectableMax.y &&
        //            selectorMax.y > selectableMin.y;
        // }
        #endregion
    }
}
