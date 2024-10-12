using UnityEngine;
using UnityEngine.Events;

namespace RTSSelector.Scripts.Runtime.Selectable
{
    using Interfaces;
    public class RTSSelectable : MonoBehaviour, IRTSSelectable
    {
        //Fields
    
        //Actions
        public event UnityAction OnSelect;
        public event UnityAction OnUnselect;
    
        //Event
        [SerializeField] private UnityEvent OnSelectEvent;
        [SerializeField] private UnityEvent OnUnselectEvent;

        private void Awake()
        {
            OnSelectEvent.AddListener(() => OnSelect?.Invoke());
            OnUnselectEvent.AddListener(() => OnUnselect?.Invoke());
        }

        public Vector2 GetScreenPos()
        {
            return Camera.main.WorldToScreenPoint(transform.position);
        }

        public void Select()
        {
            OnSelectEvent.Invoke();
        }

        public void Unselect()
        {
            OnUnselectEvent.Invoke();
        }
    }
}
