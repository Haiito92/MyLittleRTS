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
        private bool _isSelecting = false;
            
        //Selector Mesh
        // [SerializeField] private MeshFilter _selectorMeshFilter;
        // private Mesh _selectorMesh;
        // [SerializeField] private float _startDistanceFromCamera = 0.1f;
        // [SerializeField] private float _endDistanceFromCamera = 50f;
        
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
            
            //_currentSelection = _allRtsSelectables.FindAll(rtsSelectable => RectTransformUtility.RectangleContainsScreenPoint(_selectorBox, rtsSelectable.GetScreenPos()));
            _currentSelection =
                _allRtsSelectables.FindAll(rtsSelectable => _selectorBox.rect.Overlaps(rtsSelectable.GetScreenRect()));
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

        // private void UpdateSelectorMesh()
        // {
        //     Vector3[] corners = new Vector3[4];
        //     _selectorBox.GetWorldCorners(corners);
        //     
        //
        //     Ray[] rays = new Ray[4];
        //
        //     for (int i = 0; i < corners.Length; i++)
        //     {
        //         rays[i] = Camera.main.ScreenPointToRay(corners[i]);
        //     }
        //
        //     Vector3[] frontPoints = new Vector3[4];
        //     Vector3[] backPoints = new Vector3[4];
        //
        //     for (int i = 0; i < rays.Length; i++)
        //     {
        //         frontPoints[i] = rays[i].origin + rays[i].direction * _startDistanceFromCamera;
        //     }
        //     
        //     for (int i = 0; i < rays.Length; i++)
        //     {
        //         backPoints[i] = rays[i].origin + rays[i].direction * _endDistanceFromCamera;
        //     }
        //     
        //     Vector3[] newVertices = new Vector3[]
        //     {
        //         //Front
        //         frontPoints[0], frontPoints[1], frontPoints[2], frontPoints[3],
        //         
        //         //Right
        //         frontPoints[3], backPoints[3], backPoints[2], frontPoints[2],
        //         
        //         //Top
        //         frontPoints[2], backPoints[2], backPoints[1], frontPoints[1],
        //         
        //         //Left
        //         frontPoints[1], backPoints[1], backPoints[0], frontPoints[0],
        //         
        //         //Bottom
        //         frontPoints[0], frontPoints[3], backPoints[3], backPoints[0],
        //         
        //         //Back
        //         backPoints[0], backPoints[1], backPoints[2], backPoints[3]
        //     };
        //
        //     _selectorMesh = _selectorMeshFilter.mesh;
        //     _selectorMesh.SetVertices(newVertices);
        //     _selectorMesh.RecalculateBounds();
        // }
    }
}