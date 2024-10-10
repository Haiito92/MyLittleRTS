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
            Ray minRay = Camera.main.ScreenPointToRay(_selectorBox.rect.min);
            Ray topLeftRay = Camera.main.ScreenPointToRay(new Vector2(_selectorBox.rect.min.x, _selectorBox.rect.max.y));
            Ray maxRay = Camera.main.ScreenPointToRay(_selectorBox.rect.max);
            Ray bottomRightRay = Camera.main.ScreenPointToRay(new Vector2(_selectorBox.rect.max.x, _selectorBox.rect.min.y));
            
            Vector3 fbl = minRay.origin + minRay.direction * 0.1f;
            Vector3 ftl = topLeftRay.origin + topLeftRay.direction * 0.1f;
            Vector3 ftr = maxRay.origin + maxRay.direction * 0.1f;
            Vector3 fbr = bottomRightRay.origin + bottomRightRay.direction * 0.1f;

            Vector3 bbl = minRay.origin + minRay.direction * 10f;
            Vector3 btl = topLeftRay.origin + topLeftRay.direction * 10f;
            Vector3 btr = maxRay.origin + maxRay.direction * 10f;
            Vector3 bbr = bottomRightRay.origin + bottomRightRay.direction * 10f;

            _selectorMesh = _selectorMeshFilter.mesh;

            Vector3[] newVertices = new Vector3[]
            {
                //Front
                fbl, ftl, ftr, fbr,

                //Right
                fbr, bbr, btr, ftr,
                
                //Top
                ftr, btr, btl, ftl,
                
                //Left
                ftl, btl, bbl, fbl,
                
                //Bottom
                fbl, fbr, bbr, bbl,
                
                //Back
                bbl, btl, btr, bbr
            };

            _selectorMesh.vertices = newVertices;
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
