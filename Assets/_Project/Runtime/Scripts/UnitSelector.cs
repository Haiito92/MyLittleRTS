using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Project.Runtime.Scripts
{
    public class UnitSelector : MonoBehaviour
    {
        //Fields//
        //Input
        [SerializeField] private InputActionReference _select;
        
        //Selector Box
        private Vector2 _mouseStartPos;
        private Vector2 _mouseEndPos;
        [SerializeField] private RectTransform _selectorBox;
        private bool _isSelecting = false;
        private Coroutine _selectionUpdateCoroutine;
            
        //Selector Mesh
        [SerializeField] private MeshFilter _selectorMeshFilter;
        private Mesh _selectorMesh;
        [SerializeField] private float _startDistanceFromCamera = 0.1f;
        [SerializeField] private float _endDistanceFromCamera = 50f;
        
        //Selection
        private List<ISelectable> _selection;

        //Actions//
        public UnityAction OnSelectionStart;
        public UnityAction OnSelectionEnd;
        
        //UnityEvent//
        [SerializeField] private UnityEvent OnSelectionStartEvent;
        [SerializeField] private UnityEvent OnSelectionEndEvent;

        private void Awake()
        {
            OnSelectionStartEvent.AddListener(() => OnSelectionStart?.Invoke());
            OnSelectionEndEvent.AddListener(() => OnSelectionEnd?.Invoke());
        }

        private void OnSelectInputActionEvent(InputAction.CallbackContext ctx)
        {
            if(ctx.started) StartSelection();
            if(ctx.canceled) FinishSelection();
        }

        private void StartSelection()
        {
            _isSelecting = true;
            
            _mouseStartPos = Mouse.current.position.value;
            _selectorBox.anchoredPosition = _mouseStartPos;
            
            _selectorBox.gameObject.SetActive(true);
            
            OnSelectionStartEvent.Invoke();
            
            if(_selectionUpdateCoroutine != null) return;
            _selectionUpdateCoroutine = StartCoroutine(SelectionUpdate());
        }

        private IEnumerator SelectionUpdate()
        {
            while (_isSelecting)
            {
                UpdateSelectorBox();

                yield return null;
            }
        }
        
        private void FinishSelection()
        {
            if (_selectionUpdateCoroutine != null)
            {
                StopCoroutine(_selectionUpdateCoroutine);
                _selectionUpdateCoroutine = null;
            }
            
            UpdateSelectorMesh();
            
            _isSelecting = false;
            OnSelectionEndEvent.Invoke();
                        
            _selectorBox.gameObject.SetActive(false);
        }

        private void UpdateSelectorBox()
        {
            _mouseEndPos = Mouse.current.position.value;

            float width = _mouseEndPos.x - _mouseStartPos.x;
            float height = _mouseEndPos.y - _mouseStartPos.y;

            _selectorBox.anchoredPosition = _mouseStartPos + new Vector2(width / 2, height / 2);
            _selectorBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        }

        private void UpdateSelectorMesh()
        {
            Vector3[] corners = new Vector3[4];
            _selectorBox.GetWorldCorners(corners);
            

            Ray[] rays = new Ray[4];

            for (int i = 0; i < corners.Length; i++)
            {
                rays[i] = Camera.main.ScreenPointToRay(corners[i]);
            }

            Vector3[] frontPoints = new Vector3[4];
            Vector3[] backPoints = new Vector3[4];

            for (int i = 0; i < rays.Length; i++)
            {
                frontPoints[i] = rays[i].origin + rays[i].direction * _startDistanceFromCamera;
            }
            
            for (int i = 0; i < rays.Length; i++)
            {
                backPoints[i] = rays[i].origin + rays[i].direction * _endDistanceFromCamera;
            }
            
            Vector3[] newVertices = new Vector3[]
            {
                //Front
                frontPoints[0], frontPoints[1], frontPoints[2], frontPoints[3],
                
                //Right
                frontPoints[3], backPoints[3], backPoints[2], frontPoints[2],
                
                //Top
                frontPoints[2], backPoints[2], backPoints[1], frontPoints[1],
                
                //Left
                frontPoints[1], backPoints[1], backPoints[0], frontPoints[0],
                
                //Bottom
                frontPoints[0], frontPoints[3], backPoints[3], backPoints[0],
                
                //Back
                backPoints[0], backPoints[1], backPoints[2], backPoints[3]
            };
            
            // Ray minRay = Camera.main.ScreenPointToRay(corners[0]);
            // Ray topLeftRay = Camera.main.ScreenPointToRay(corners[1]);
            // Ray maxRay = Camera.main.ScreenPointToRay(corners[2]);
            // Ray bottomRightRay = Camera.main.ScreenPointToRay(corners[3]);
            
            // Vector3 fbl = minRay.origin + minRay.direction * _startDistanceFromCamera;
            // Vector3 ftl = topLeftRay.origin + topLeftRay.direction * _startDistanceFromCamera;
            // Vector3 ftr = maxRay.origin + maxRay.direction * _startDistanceFromCamera;
            // Vector3 fbr = bottomRightRay.origin + bottomRightRay.direction * _startDistanceFromCamera;
            //
            // Vector3 bbl = minRay.origin + minRay.direction * _endDistanceFromCamera;
            // Vector3 btl = topLeftRay.origin + topLeftRay.direction * _endDistanceFromCamera;
            // Vector3 btr = maxRay.origin + maxRay.direction * _endDistanceFromCamera;
            // Vector3 bbr = bottomRightRay.origin + bottomRightRay.direction * _endDistanceFromCamera;
            //
            // _selectorMesh = _selectorMeshFilter.mesh;
            //
            // Vector3[] newVertices = new Vector3[]
            // {
            //     //Front
            //     fbl, ftl, ftr, fbr,
            //
            //     //Right
            //     fbr, bbr, btr, ftr,
            //     
            //     //Top
            //     ftr, btr, btl, ftl,
            //     
            //     //Left
            //     ftl, btl, bbl, fbl,
            //     
            //     //Bottom
            //     fbl, fbr, bbr, bbl,
            //     
            //     //Back
            //     bbl, btl, btr, bbr
            // };

            _selectorMesh = _selectorMeshFilter.mesh;
            _selectorMesh.SetVertices(newVertices);
            _selectorMesh.RecalculateBounds();
        }
        
        private void OnEnable()
        {
            if(_select != null)
            {
                _select.action.started += OnSelectInputActionEvent;
                _select.action.canceled += OnSelectInputActionEvent;
            }
        }

        private void OnDisable()
        {
            if(_select != null)
            {
                _select.action.started -= OnSelectInputActionEvent;
                _select.action.canceled -= OnSelectInputActionEvent;
            }
        }
    }
}
