using RTSSelector.Scripts.Runtime.Core;
using Unity.Entities;
using UnityEngine;

namespace RTSSelector.Scripts.Runtime.ECS.Core
{
    partial struct RTSSelectableSystem : ISystem
    {
        partial struct UpdateSelectableRect : IJobEntity
        {
            void Execute(ref RTSSelectableComponent rtsSelectableComponent)
            {
                rtsSelectableComponent.CachedRTSScreenRect = GetScreenRect(rtsSelectableComponent.ColliderRef.Value);
            }
        }

        public void OnUpdate(ref SystemState state)
        {
            // new UpdateSelectableRect().ScheduleParallel();
            // foreach (RTSSelectableComponent rtsSelectableComponent in SystemAPI.Query<RTSSelectableComponent>())
            // {
            //     Debug.Log(rtsSelectableComponent.ColliderRef.Value.gameObject.name);
            // }
        }
        
        static RTSScreenRect GetScreenRect(Collider collider)
        {
            Vector3[] boundsVertices = new Vector3[8];

            Vector3 vMin = collider.bounds.min;
            Vector3 vMax = collider.bounds.max;
            
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
    }
}
