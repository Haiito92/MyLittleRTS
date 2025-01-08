using Unity.Entities;
using UnityEngine;

namespace RTSSelector.Scripts.ECS.Runtime.Core
{
    public struct RTSSelectableComponent : IComponentData
    {
        public UnityObjectRef<Collider> Collider;

        public RTSSelectableComponent(Collider inCollider)
        {
            Collider = inCollider;
        }
    }
}
