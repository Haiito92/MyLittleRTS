using Unity.Transforms;
using Unity.Entities;
using Unity.Rendering;

namespace _Project.Scripts.Runtime.Core
{
    partial struct GameSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            // Entity unitEntity = state.EntityManager.CreateEntity();
            //
            // state.EntityManager.AddComponent<LocalTransform>(unitEntity);
            // state.EntityManager.AddComponent<RenderMeshUnmanaged>(unitEntity);
            // state.EntityManager.AddComponent<Prefab>(unitEntity);
            //
            // for (int i = 0; i < 2; i++)
            // {
            //     Entity copyOfUnitEntity = state.EntityManager.Instantiate(unitEntity);
            // }
        }
    }
}
