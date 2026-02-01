using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct SmoothAnimationInitSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(SystemAPI.QueryBuilder()
            .WithAll<EntityInstanceInitRequest, EntityInstance, SmoothAnimationInitData>()
            .Build()
        );
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI
            .GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        new SpawnJob
        {
            Ecb = ecb,
        }.ScheduleParallel();
    }

    [BurstCompile]
    [WithAll(typeof(EntityInstanceInitRequest))]
    partial struct SpawnJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter Ecb;

        public void Execute(
            [EntityIndexInQuery] int index,
            in EntityInstance prefab,
            in SmoothAnimationInitData init,
            Entity entity
        )
        {
            var instance = prefab.Value;
            var data = init;

            Ecb.SetComponent(index, instance, new SmoothAnimationPlayer
            {
                Speed = data.Speed,
                Duration = data.Duration,
                Progress = 0f,
                Reverse = data.Reverse,
                Loop = data.Loop,
                Alternate = data.Alternate,
            });
        }
    }
}
