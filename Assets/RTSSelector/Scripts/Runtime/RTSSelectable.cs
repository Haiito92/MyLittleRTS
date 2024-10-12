using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RTSSelector.Scripts.Runtime
{
    public class RTSSelectable : MonoBehaviour, IRTSSelectable
    {
        //Fields
    
        //Actions
        public event UnityAction Selected;
        public event UnityAction Unselected;
    
        //Event
        [SerializeField] private UnityEvent SelectedEvent;
        [SerializeField] private UnityEvent UnselectedEvent;

        private void Awake()
        {
            SelectedEvent.AddListener(() => Selected?.Invoke());
            UnselectedEvent.AddListener(() => Unselected?.Invoke());
        }

        public Vector2 GetScreenPos()
        {
            return Camera.main.WorldToScreenPoint(transform.position);
        }

        public void Select()
        {
            SelectedEvent.Invoke();
        }

        public void Unselect()
        {
            UnselectedEvent.Invoke();
        }
    }
}
