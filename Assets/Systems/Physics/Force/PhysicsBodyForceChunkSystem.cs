using NUnit.Framework;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsBatchForceCreationSystem))]
partial struct PhysicsForceChunkingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsBodyForce>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var forceQuery = SystemAPI.QueryBuilder()
            .WithAll<PhysicsBodyForce>()
            .Build();

        var batchForceBuffer = SystemAPI.GetSingletonBuffer<PhysicsBodyForceChunkCount>();
        batchForceBuffer.ResizeUninitialized(forceQuery.CalculateChunkCount());

        var job = new CountChunkForcesJob
        {
            ChunkCounts = batchForceBuffer.Reinterpret<int>().AsNativeArray(),
            ForceHandle = SystemAPI.GetComponentTypeHandle<PhysicsBodyForce>()
        };

        state.Dependency = job.ScheduleParallel(forceQuery, state.Dependency);
    }

    [BurstCompile]
    partial struct CountChunkForcesJob : IJobChunk
    {
        [ReadOnly] public ComponentTypeHandle<PhysicsBodyForce> ForceHandle;
        public NativeArray<int> ChunkCounts;

        public void Execute(
            in ArchetypeChunk chunk,
            int chunkIndex,
            bool useEnabledMask,
            in v128 chunkEnabledMask
        )
        {
            Assert.IsFalse(useEnabledMask);
            var forces = chunk.GetNativeArray(ref ForceHandle);

            int count = 0;
            for (int i = 0, entityCount = chunk.Count; i < entityCount; i++)
            {
                if (math.any(forces[i].Force != float2.zero)) count += 1;
            }

            ChunkCounts[chunkIndex] = count;
        }
    }
}

