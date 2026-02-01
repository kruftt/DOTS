using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct FlipbookAnimationInitSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(SystemAPI.QueryBuilder()
            .WithAll<EntityInstanceInitRequest, FlipbookAnimationInitData, EntityInstance>()
            .Build()
        );
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI
            .GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        new SpawnJob { ecb = ecb }.ScheduleParallel();
    }

    [BurstCompile]
    [WithAll(typeof(EntityInstanceInitRequest))]
    partial struct SpawnJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        void Execute(
            [EntityIndexInChunk] int index,
            in EntityInstance prefab,
            in FlipbookAnimationInitData initData,
            Entity entity
        )
        {
            var instance = prefab.Value;

            ecb.SetComponent(index, instance,
                new FlipbookAnimationPlayer
                {
                    Clip = initData.Clip,
                    Progress = 0f,
                    Speed = initData.Speed
                }
            );
        }
    }
}