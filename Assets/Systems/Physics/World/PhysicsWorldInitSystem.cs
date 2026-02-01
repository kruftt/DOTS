using Unity.Burst;
using Unity.Entities;
using UnityEngine.LowLevelPhysics2D;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateBefore(typeof(PhysicsBodyInitSystem))]
partial struct PhysicsWorldInitSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldInitData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var app = SystemAPI.GetSingletonEntity<AppTag>();
        var em = state.EntityManager;
        var ecb = SystemAPI
            .GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (data, entity) in SystemAPI.Query
            <RefRO<PhysicsWorldInitData>>()
            .WithEntityAccess())
        {
            if (em.HasComponent<PhysicsWorldHandle>(app))
            {
                var currentHandle = em.GetComponentData<PhysicsWorldHandle>(app);
                if (currentHandle.World.isValid)
                {
                    currentHandle.World.Destroy();
                }
            }
            else
            {
                ecb.AddComponent<PhysicsWorldHandle>(app);
            }

            ecb.SetComponent(app, new PhysicsWorldHandle
            {
                World = PhysicsWorld.Create(data.ValueRO.Definition)
            });

            if (!em.HasBuffer<PhysicsBatchForce>(app))
            {
                ecb.AddBuffer<PhysicsBatchForce>(app);
            }

            if (!em.HasBuffer<PhysicsBodyForceChunkCount>(app))
            {
                ecb.AddBuffer<PhysicsBodyForceChunkCount>(app);
            }

            ecb.DestroyEntity(entity);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        foreach (var handle in SystemAPI.Query<RefRO<PhysicsWorldHandle>>())
        {
            if (handle.ValueRO.World.isValid)
            {
                handle.ValueRO.World.Destroy();
            }
        }
    }
}
