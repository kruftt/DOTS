using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct TransientSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(
            SystemAPI.QueryBuilder().WithAny<TransientTicker, TransientTimer>().Build()
        );
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI
            .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged)
            .AsParallelWriter();

        state.Dependency = new CleanupEventsJob { ecb = ecb }.ScheduleParallel(state.Dependency);
        state.Dependency = JobHandle.CombineDependencies(
            new DestroyTickersJob { ecb = ecb }.ScheduleParallel(state.Dependency),
            new DestroyTimersJob { dt = SystemAPI.Time.DeltaTime, ecb = ecb }.ScheduleParallel(state.Dependency)
        );
    }

    [BurstCompile]
    partial struct CleanupEventsJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute(
            [EntityIndexInQuery] int index,
            ref TransientExpiredEvent evt,
            Entity entity
        )
        {
            ecb.DestroyEntity(index, entity);
        }
    }

    [BurstCompile]
    partial struct DestroyTimersJob : IJobEntity
    {
        public float dt;
        public EntityCommandBuffer.ParallelWriter ecb;

        public void Execute(
            [EntityIndexInQuery] int index,
            ref TransientTimer timer,
            Entity entity
        )
        {
            timer.Lifetime -= dt;
            if (timer.Lifetime <= 0f)
            {
                if (timer.EmitEvent)
                {
                    var evt = ecb.CreateEntity(index);
                    ecb.AddComponent<TransientExpiredEvent>(index, evt);
                    ecb.SetComponent(index, evt, new TransientExpiredEvent { Entity = entity });
                }

                ecb.DestroyEntity(index, entity);
            }
        }
    }

    [BurstCompile]
    partial struct DestroyTickersJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;

        public void Execute(
            [EntityIndexInQuery] int index,
            ref TransientTicker ticker,
            Entity entity
        )
        {
            ticker.Lifetime -= 1;

            if (ticker.Lifetime < 1)
            {
                if (ticker.EmitEvent)
                {
                    var evt = ecb.CreateEntity(index);
                    ecb.AddComponent<TransientExpiredEvent>(index, evt);
                    ecb.SetComponent(index, evt, new TransientExpiredEvent { Entity = entity });
                }

                ecb.DestroyEntity(index, entity);
            }
        }
    }
}
