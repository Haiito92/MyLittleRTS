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
                _mouseEndPos = Mouse.current.position.value;

                float width = _mouseEndPos.x - _mouseStartPos.x;
                float height = _mouseEndPos.y - _mouseStartPos.y;

                _selectorBox.anchoredPosition = _mouseStartPos + new Vector2(width / 2, height / 2);
                _selectorBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

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
            
            _isSelecting = false;
            OnSelectionEndEvent.Invoke();
                        
            _selectorBox.gameObject.SetActive(false);
            
            _mouseEndPos = Mouse.current.position.value;
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
