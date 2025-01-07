using Unity.Entities;
using UnityEngine;

namespace _Project.Scripts.Runtime.Health
{
    
    
    partial struct HealthSystem : ISystem
    {
        
        partial struct UpdateHealthJob : IJobEntity
        {
            void Execute(ref HealthComponent healthComponent)
            {
                Debug.Log(healthComponent.Health);
            }
        }

        public void OnUpdate(ref SystemState state)
        {
            new UpdateHealthJob().ScheduleParallel();
        }
    }
}
