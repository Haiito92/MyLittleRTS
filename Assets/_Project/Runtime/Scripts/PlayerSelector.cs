using System;
using System.Collections.Generic;
using _Project.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Runtime.Scripts
{
    public class PlayerSelector : MonoBehaviour
    {
        [SerializeField] private InputActionReference _select;

        private List<ISelectable> _selection;
        
        private void OnSelectInputActionEvent(InputAction.CallbackContext ctx)
        {
            if(ctx.started) StartSelection();
            if(ctx.canceled) FinishSelection();
        }

        private void StartSelection()
        {

            Ray selectRay = Camera.main.ScreenPointToRay(Mouse.current.position.value);
            
            if(!Physics.Raycast(selectRay, out RaycastHit selectInfo, 1000.0f)) return;
            
            //Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            //Debug.DrawLine(mouseWorldPos, selectInfo.point, Color.red, 10f);
            
            if(selectInfo.collider.TryGetComponent(out ISelectable selectable))
            {
                selectable.Select();
            }
        }

        private void FinishSelection()
        {
            
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
