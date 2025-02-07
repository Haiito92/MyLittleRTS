using RTSSelector.Scripts.Runtime.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RTSSelector.Scripts.Runtime.ECS.Systems
{

    public partial struct RTSTransformSystem : ISystem
    {
        partial struct UpdateTransformComponentJob : IJobEntity
        {
            void Execute(in LocalTransform transform, ref RTSTransformComponent rtsTransformComponent)
            {
                rtsTransformComponent.Moved = !transform.Position.Equals(rtsTransformComponent.LastPosition);

                rtsTransformComponent.LastPosition = transform.Position;
                
                Debug.Log(rtsTransformComponent.Moved);
            }
        }

        public void OnUpdate(ref SystemState state)
        {
            // UpdateTransformComponentJob updateTransformComponentJob = new UpdateTransformComponentJob();
            new UpdateTransformComponentJob().ScheduleParallel();
        }
    }
}
