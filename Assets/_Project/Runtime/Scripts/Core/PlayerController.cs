using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Runtime.Scripts.Core
{
    using RTSSelector.Scripts.Runtime.Selector;

    public class PlayerController : MonoBehaviour
    {
        //Input
        [SerializeField] private InputActionReference _select;

        private RTSSelector _rtsSelector;
        
        private void Start()
        {
            _rtsSelector = RTSSelector.Instance;
        }
    
        private void OnSelectInputActionEvent(InputAction.CallbackContext ctx)
        {
            if(ctx.started) _rtsSelector.StartSelection(Mouse.current.position.value);
            if(ctx.canceled) _rtsSelector.FinishSelection();
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
