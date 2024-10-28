using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Runtime.Units.Characters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Runtime.Core
{
    using RTSSelector.Scripts.Runtime.Core;
    public class PlayerController : MonoBehaviour
    {
        //Inputs
        [SerializeField] private InputActionReference _select;
        [SerializeField] private InputActionReference _mouseMove;

        //Selection
        private RTSSelector _rtsSelector;
        private List<RTSSelectable> _selection;

        private void Awake()
        {
            _selection = new List<RTSSelectable>();
        }

        private void Start()
        {
            _rtsSelector = RTSSelector.Instance;
        }
    
        private void OnSelectInputActionEvent(InputAction.CallbackContext ctx)
        {
            if(ctx.started)
            {
                _rtsSelector.StartSelection(_mouseMove.action.ReadValue<Vector2>());
            }
            
            if(ctx.canceled)
            {
                _selection.ForEach(rtsSelectable => rtsSelectable.Unselect());
                
                List<RTSSelectable> newSelection = _rtsSelector.FinishSelection();

                foreach (RTSSelectable rtsSelectable in newSelection)
                {
                    if(!rtsSelectable.TryGetComponent(out CharacterUnit characterUnit)) continue;
                    
                    _selection.Add(rtsSelectable);
                }
                
                _selection.ForEach(rtsSelectable => rtsSelectable.Select());
            }
        }

        private void OnMouseMoveInputActionEvent(InputAction.CallbackContext ctx)
        {
            if(ctx.performed)
            {
                if(_rtsSelector.IsSelecting)_rtsSelector.UpdatePreselection(ctx.ReadValue<Vector2>());
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
