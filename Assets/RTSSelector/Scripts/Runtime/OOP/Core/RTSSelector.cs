using System.Collections.Generic;
using RTSSelector.Scripts.Runtime.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RTSSelector.Scripts.Runtime.OOP.Core
{
    public class RTSSelector : MonoBehaviour
    {
        //Fields//
        //Selectables
        private List<RTSSelectable> _allRtsSelectables;
        //private List<RTSSelectable> _currentSelection;
        
        //Selector Box
        private Vector2 _mouseStartPos;
        private Vector2 _mouseEndPos;
        private RTSScreenRect _selectorRect;
        private bool _isSelecting = false;
        
        //Actions//
        public UnityAction SelectionStarted;
        public UnityAction SelectionUpdated;
        public UnityAction SelectionEnded;
        
        //UnityEvent//
        [SerializeField] private UnityEvent SelectionStartedEvent;
        [SerializeField] private UnityEvent SelectionUpdatedEvent;
        [SerializeField] private UnityEvent SelectionEndedEvent;


        #region Properties
        public List<RTSSelectable> AllRtsSelectables => _allRtsSelectables;
        public RTSScreenRect SelectorRect => _selectorRect;
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
            //_currentSelection = new List<RTSSelectable>();
            
            //Subscribe UnityActions Invoke to their corresponding UnityEvent
            SelectionStartedEvent.AddListener(() => SelectionStarted?.Invoke());
            SelectionUpdatedEvent.AddListener(() => SelectionUpdated?.Invoke());
            SelectionEndedEvent.AddListener(() => SelectionEnded?.Invoke());
        }

        #region Selection
        public void StartSelection(Vector2 mouseStartPos)
        {
            // if (_currentSelection != null)
            // {
            //     _currentSelection.ForEach(rtsSelectable => rtsSelectable.Unselect());
            //     _currentSelection.Clear();
            // }
            
            _isSelecting = true;
            
            // _mouseStartPos = mouseStartPos * 1/_canvas.scaleFactor;
            _mouseStartPos = mouseStartPos;

            _selectorRect = new RTSScreenRect(_mouseStartPos.x, _mouseStartPos.y, 0f, 0f);
            
            SelectionStartedEvent.Invoke();
        }

        public List<RTSSelectable> UpdatePreselection(Vector2 currentMousePos)
        {
            UpdateSelectorRect(currentMousePos);

            List<RTSSelectable> preSelection = new List<RTSSelectable>();
            
            foreach (RTSSelectable rtsSelectable in _allRtsSelectables)
            {
                RTSScreenRect selectableRect = rtsSelectable.GetScreenRect();
                if (_selectorRect.Overlaps(selectableRect) && !preSelection.Contains(rtsSelectable))
                {
                    preSelection.Add(rtsSelectable);
                    continue;
                }
                if(!_selectorRect.Overlaps(selectableRect) && preSelection.Contains(rtsSelectable))
                {
                    preSelection.Remove(rtsSelectable);
                }
            }
            
            SelectionUpdatedEvent.Invoke();

            return preSelection;
        }
        
        public List<RTSSelectable> FinishSelection(Vector2 mouseEndPos)
        {
            _isSelecting = false;
            UpdateSelectorRect(mouseEndPos);
            
            List<RTSSelectable> selection = new List<RTSSelectable>();
            
            //iterate to see which selectable are in rectangle
            foreach (RTSSelectable rtsSelectable in _allRtsSelectables)
            {
                RTSScreenRect selectableRect = rtsSelectable.GetScreenRect();
                if (_selectorRect.Overlaps(selectableRect)) selection.Add(rtsSelectable);
            }
            
            SelectionEndedEvent.Invoke();
            
            return selection;
        }
        #endregion
        
        private RTSScreenRect UpdateSelectorRect(Vector2 mouseCurrentPos)
        {
            _mouseEndPos = mouseCurrentPos;

            float XMin = Mathf.Min(_mouseStartPos.x, _mouseEndPos.x);
            float XMax = Mathf.Max(_mouseStartPos.x, _mouseEndPos.x);
            
            float YMin = Mathf.Min(_mouseStartPos.y, _mouseEndPos.y);
            float YMax = Mathf.Max(_mouseStartPos.y, _mouseEndPos.y);

            float width = XMax - XMin;
            float height = YMax - YMin;

            _selectorRect = new RTSScreenRect(XMin, YMin, width, height);

            return _selectorRect;
        }

        
    }
}
