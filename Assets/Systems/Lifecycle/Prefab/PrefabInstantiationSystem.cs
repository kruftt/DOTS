using Unity.Burst;
using Unity.Entities;
using Unity.Scenes;

[UpdateInGroup(typeof(PresentationSystemGroup))]
partial struct PrefabInstantiationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(
            SystemAPI.QueryBuilder()
                .WithAll<PrefabInstanceRequest, PrefabLoadResult>()
                .WithNone<EntityInstance, EntityInstanceInitRequest>()
                .Build()
            );
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI
            .GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        new SpawnJob { ecb = ecb }.ScheduleParallel();
    }

    [BurstCompile]
    [WithAll(typeof(PrefabLoadResult))]
    [WithNone(typeof(EntityInstance), typeof(EntityInstanceInitRequest))]
    partial struct SpawnJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;

        void Execute(
            [EntityIndexInChunk] int index,
            in PrefabInstanceRequest request,
            in PrefabLoadResult loadResult,
            Entity entity
        )
        {
            var instance = ecb.Instantiate(index, loadResult.PrefabRoot);
            if (request.IsStatic)
            {
                ecb.AddComponent(index, instance, new TransformStaticTag { });
            }

            ecb.AddComponent(index, entity, new EntityInstance
            {
                Value = instance
            });

            ecb.AddComponent(index, entity, new EntityInstanceInitRequest { });
            ecb.AddComponent(index, entity, new TransientTicker
            {
                Lifetime = 1,
                EmitEvent = false
            });

            ecb.RemoveComponent<RequestEntityPrefabLoaded>(index, entity);
            ecb.RemoveComponent<PrefabLoadResult>(index, entity);
        }
    }
}
