using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RTSSelector.Scripts.Runtime
{
    public class RTSSelectable : MonoBehaviour, IRTSSelectable
    {
        //Fields
        [SerializeField] private Collider _collider;
    
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

        private void Start()
        {
            RTSSelector.Instance.AllRtsSelectables.Add(this);
        }

        public Vector2 GetScreenPos()
        {
            return Camera.main.WorldToScreenPoint(transform.position);
        }

        public Rect GetScreenRect()
        {
            Vector3[] boundVertices = new Vector3[8];
            Vector3 boundsMin = _collider.bounds.min;
            Vector3 boundsMax = _collider.bounds.max;
                
            Vector2[] points = new Vector2[8];
            foreach (var VARIABLE in COLLECTION)
            {
                
            }
            
            return new Rect();
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
