using _Project.Scripts.Runtime.Health;
using RTSSelector.Scripts.Runtime.ECS.Components;
using Unity.Transforms;
using UnityEngine;

namespace _Project.Scripts.Runtime.Units
{
    using Unity.Entities;
    class UnitBaker : Baker<UnitAuthoring>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            // Debug.Log(authoring.Health);
            // AddComponent(entity, new HealthComponent(authoring.Health));
            AddComponent(entity, new RTSTransformComponent());
            AddComponent(entity, new RTSSelectableComponent(authoring.Collider));
        }
    }
}
 