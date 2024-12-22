using Unity.Transforms;
using UnityEngine;

namespace _Project.Scripts.Runtime.Units
{
    using Unity.Entities;
    public class UnitBaker : Baker<UnitAuthoring>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<LocalTransform>(entity);
        }
    }
}
