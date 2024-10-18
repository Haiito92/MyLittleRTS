using UnityEngine;

namespace RTSSelector.Scripts.Runtime
{
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