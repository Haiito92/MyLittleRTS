using Unity.Entities;

namespace _Project.Scripts.Runtime.Health
{
    public struct HealthComponent : IComponentData
    {
        public float Health;

        public HealthComponent(float inHealth)
        {
            Health = inHealth;
        }
    }
}
