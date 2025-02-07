using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace RTSSelector.Scripts.Runtime.ECS.Components
{
    public struct RTSTransformComponent : IComponentData
    {
        public float3 LastPosition;
        public bool Moved;
    }
}
