using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Canvas _canvas;
        public RectTransform RectTransform => _rectTransform;
        
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
            RTSScreenRect rect = GetScreenRect();
            _rectTransform.anchoredPosition = rect.Center ;
            _rectTransform.sizeDelta = new Vector2(rect.Width, rect.Height);
        }

        public Vector2 GetScreenPos()
        {
            return Camera.main.WorldToScreenPoint(transform.position);
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
                points[i] = Camera.main.WorldToScreenPoint(boundsVertices[i]) * 1/_canvas.scaleFactor;
                // points[i] = Camera.main.WorldToScreenPoint(boundsVertices[i]);
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
            
            //Rect screenRect = new Rect(Xmin, Ymin, (Xmax - Xmin), (Ymax - Ymin));
            return rect;
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

    public struct RTSScreenRect
    {
        public Vector2 Min;
        public Vector2 Max;
        public Vector2 Center;
        
        public float Width;
        public float Height;

        public RTSScreenRect(Vector2 min, float width, float height)
        {
            Width = Mathf.Abs(width);
            Height = Mathf.Abs(height);
            
            Min = min;
            Max = new Vector2(min.x + Width, min.y + Height);
            Center = new Vector2(min.x + Width/2f, min.y + Height/2f);

        }
        
        public RTSScreenRect(float xMin, float yMin, float width, float height)
        {
            Width = Mathf.Abs(width);
            Height = Mathf.Abs(height);
            
            Min = new Vector2(xMin, yMin);
            Max = new Vector2(xMin + Width, yMin + Height);
            Center = new Vector2(xMin + Width/2f, yMin + Height/2f);

        }

        public bool Overlaps(RTSScreenRect other)
        {
            return Min.x < other.Max.x &&
                   Max.x > other.Min.x &&
                   Min.y < other.Max.y &&
                   Max.y > other.Min.y;
        }
    }
}
