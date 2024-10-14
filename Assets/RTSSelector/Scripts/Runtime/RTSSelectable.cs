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

        //TEST
        Vector3[] _boundsVertices = new Vector3[8];
        private Rect _rect;
        [SerializeField] private RectTransform _rectTransform;
        
        private void Awake()
        {
            SelectedEvent.AddListener(() => Selected?.Invoke());
            UnselectedEvent.AddListener(() => Unselected?.Invoke());
        }

        private void Start()
        {
            RTSSelector.Instance.AllRtsSelectables.Add(this);
        }

        private void Update()
        {
            // //TEST
            // if(_collider == null) return;
            //
            // Vector3 vMin = _collider.bounds.min;
            // Vector3 vMax = _collider.bounds.max;
            //
            // //MinMax
            // _boundsVertices[0] = vMin;
            // _boundsVertices[6] = vMax;
            // //Bottom
            // _boundsVertices[1] = new Vector3(vMax.x, vMin.y, vMin.z);
            // _boundsVertices[2] = new Vector3(vMax.x, vMin.y, vMax.z);
            // _boundsVertices[3] = new Vector3(vMin.x, vMin.y, vMax.z);
            // //Top
            // _boundsVertices[4] = new Vector3(vMin.x, vMax.y, vMin.z);
            // _boundsVertices[5] = new Vector3(vMax.x, vMax.y, vMin.z);
            // _boundsVertices[7] = new Vector3(vMin.x, vMax.y, vMax.z);

            _rect = GetScreenRect();
            _rectTransform.anchoredPosition = _rect.center;
            _rectTransform.sizeDelta = new Vector2(Mathf.Abs(_rect.width), Mathf.Abs(_rect.height));
            //Debug.Log(_rect.size);
        }

        public Vector2 GetScreenPos()
        {
            return Camera.main.WorldToScreenPoint(transform.position);
        }


        public Rect GetScreenRect()
        {
            Vector3[] boundsVertices = new Vector3[8];

            Vector3 vMin = _collider.bounds.min;
            Vector3 vMax = _collider.bounds.max;
            
            //MinMax
            boundsVertices[0] = vMin;
            boundsVertices[6] = vMax;
            //Bottom
            boundsVertices[1] = new Vector3(vMax.x, vMin.y, vMin.z);
            boundsVertices[2] = new Vector3(vMax.x, vMin.y, vMax.z);
            boundsVertices[3] = new Vector3(vMin.x, vMin.y, vMax.z);
            //Top
            boundsVertices[4] = new Vector3(vMin.x, vMax.y, vMin.z);
            boundsVertices[5] = new Vector3(vMax.x, vMax.y, vMin.z);
            boundsVertices[7] = new Vector3(vMin.x, vMax.y, vMax.z);
            
                
            Vector2[] points = new Vector2[8];
            for (int i = 0; i < boundsVertices.Length; i++)
            {
                points[i] = Camera.main.WorldToScreenPoint(boundsVertices[i]);
            }

            float Xmin = 0, Xmax = 0, Ymin = 0, Ymax = 0;

            for (int i = 0; i < points.Length; i++)
            {
                if (i == 0)
                {
                    Xmin = points[i].x;
                    Xmax = points[i].x;
                    Ymin = points[i].y;
                    Ymax = points[i].y;
                }
                else
                {
                    if (points[i].x < Xmin) Xmin = points[i].x;
                    if (points[i].x > Xmax) Xmax = points[i].x;
                    if (points[i].y < Ymin) Ymin = points[i].y;
                    if (points[i].y > Ymax) Ymax = points[i].y;
                }
            }
            
            return new Rect(Xmin, Ymin, Xmax - Xmin, Ymax - Ymin);
        }

        public void Select()
        {
            SelectedEvent.Invoke();
        }

        public void Unselect()
        {
            UnselectedEvent.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            foreach (Vector3 vertex in _boundsVertices)
            {
                Gizmos.DrawWireSphere(vertex, 0.5f);
            }
            
            Gizmos.DrawWireSphere(new Vector2(_rect.xMin, _rect.yMin), 0.5f);            
            Gizmos.DrawWireSphere(new Vector2(_rect.xMax, _rect.yMax), 0.5f);            
            Gizmos.DrawWireSphere(new Vector2(_rect.xMin, _rect.yMax), 0.5f);            
            Gizmos.DrawWireSphere(new Vector2(_rect.xMax, _rect.yMin), 0.5f);            
        }
    }
}
