using RTSSelector.Scripts.Runtime.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RTSSelector.Scripts.Runtime.OOP.Core
{
    public class RTSSelectable : MonoBehaviour, IRTSSelectable
    {
        //Fields
        [SerializeField] private Collider _collider;
    
        //Actions
        public event UnityAction PreSelected;
        public event UnityAction Selected;

        public event UnityAction PreUnselected;
        public event UnityAction Unselected;
    
        //Event
        [SerializeField] private UnityEvent PreSelectedEvent;
        [SerializeField] private UnityEvent SelectedEvent;

        [SerializeField] private UnityEvent PreUnselectedEvent;
        [SerializeField] private UnityEvent UnselectedEvent;

        //TEST
        // [SerializeField] private RectTransform _rectTransform;
        // [SerializeField] private Canvas _canvas;
        // public RectTransform RectTransform => _rectTransform;
        
        private void Awake()
        {
            PreUnselectedEvent.AddListener(() => PreSelected?.Invoke());
            SelectedEvent.AddListener(() => Selected?.Invoke());
            
            PreUnselectedEvent.AddListener(() => PreUnselected?.Invoke());
            UnselectedEvent.AddListener(() => Unselected?.Invoke());
        }

        private void Start()
        {
            Scripts.Runtime.OOP.Core.RTSSelector.Instance.AllRtsSelectables.Add(this);
        }

        private void Update()
        {
            //TEST
            // RTSScreenRect rect = GetScreenRect();
            // _rectTransform.anchoredPosition = rect.Center ;
            // _rectTransform.sizeDelta = new Vector2(rect.Width, rect.Height);
        }

        public RTSScreenRect GetScreenRect()
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
                // points[i] = Camera.main.WorldToScreenPoint(boundsVertices[i]) * 1/_canvas.scaleFactor;
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

            RTSScreenRect rect = new RTSScreenRect(Xmin, Ymin, (Xmax - Xmin), (Ymax - Ymin));
            return rect;
        }

        public void PreSelect()
        {
            PreSelectedEvent.Invoke();
        }

        public void Select()
        {
            SelectedEvent.Invoke();
        }

        public void PreUnselect()
        {
            PreUnselectedEvent.Invoke();
        }

        public void Unselect()
        {
            UnselectedEvent.Invoke();
        }
    }

    
}
