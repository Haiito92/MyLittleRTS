using RTSSelector.Scripts.Runtime.Core;
using Unity.Entities;
using UnityEngine;

namespace RTSSelector.Scripts.Runtime.ECS.Components
{
    public struct RTSSelectableComponent : IComponentData
    {
        public UnityObjectRef<Collider> ColliderRef;
        
        public RTSScreenRect CachedRTSScreenRect;

        public RTSSelectableComponent(Collider inCollider)
        {
            ColliderRef = inCollider;
            CachedRTSScreenRect = new RTSScreenRect();
        }
    }
}
