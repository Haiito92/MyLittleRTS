using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Runtime.Core
{
    public class PlayerController : MonoBehaviour
    {
        //Inputs
        [SerializeField] private InputActionReference _select;
        [SerializeField] private InputActionReference _mouseMove;

        //Selection
        private RTSSelector.Scripts.Runtime.Core.RTSSelector _rtsSelector;
        
        private void Start()
        {
            _rtsSelector = RTSSelector.Scripts.Runtime.Core.RTSSelector.Instance;
        }
    
        private void OnSelectInputActionEvent(InputAction.CallbackContext ctx)
        {
            if(ctx.started)
            {
                _rtsSelector.StartSelection(_mouseMove.action.ReadValue<Vector2>());
            }
            
            if(ctx.canceled)
            {
                _rtsSelector.FinishSelection();
            }
        }

        private void OnMouseMoveInputActionEvent(InputAction.CallbackContext ctx)
        {
            if(ctx.performed)
            {
                if(_rtsSelector.IsSelecting) _rtsSelector.UpdatePreselection(ctx.ReadValue<Vector2>());
            }
        }

        #region OnEnable/OnDisable
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
        #endregion
    }
}
