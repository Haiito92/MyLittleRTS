using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Runtime.Scripts.Core
{
    using RTSSelector.Scripts.Runtime.Selector;

    public class PlayerController : MonoBehaviour
    {
        //Inputs
        [SerializeField] private InputActionReference _select;
        [SerializeField] private InputActionReference _mouseMove;

        //Selection
        private RTSSelector _rtsSelector;
        private bool _isSelecting;
        
        private void Start()
        {
            _rtsSelector = RTSSelector.Instance;
        }
    
        private void OnSelectInputActionEvent(InputAction.CallbackContext ctx)
        {
            if(ctx.started)
            {
                _rtsSelector.StartSelection(_mouseMove.action.ReadValue<Vector2>());
                _isSelecting = true;
            }
            
            if(ctx.canceled)
            {
                _isSelecting = false;
                _rtsSelector.FinishSelection();
            }
        }

        private void OnMouseMoveInputActionEvent(InputAction.CallbackContext ctx)
        {
            if(ctx.performed)
            {
                if(_isSelecting)_rtsSelector.UpdateSelection(ctx.ReadValue<Vector2>());
            }
        }
    
        private void OnEnable()
        {
            if(_select != null)
            {
                _select.action.started += OnSelectInputActionEvent;
                _select.action.canceled += OnSelectInputActionEvent;
            }
            
            if (_mouseMove != null)
            {
                _mouseMove.action.performed += OnMouseMoveInputActionEvent;
            }
        }

        private void OnDisable()
        {
            if(_select != null)
            {
                _select.action.started -= OnSelectInputActionEvent;
                _select.action.canceled -= OnSelectInputActionEvent;
            }

            if (_mouseMove != null)
            {
                _mouseMove.action.performed -= OnMouseMoveInputActionEvent;
            }
        }
    }
}
