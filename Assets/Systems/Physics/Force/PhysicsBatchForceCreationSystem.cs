using NUnit.Framework;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.LowLevelPhysics2D;

[UpdateInGroup(typeof(PhysicsForcePassSystemGroup))]
[UpdateAfter(typeof(PhysicsForceChunkingSystem))]
[UpdateBefore(typeof(PhysicsBatchForceSubmissionSystem))]
partial struct PhysicsBatchForceCreationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsSimulationFlag>();
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<PhysicsBodyForce, PhysicsBodyHandle>().Build());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.IsComponentEnabled<PhysicsSimulationFlag>(SystemAPI.GetSingletonEntity<AppTag>())) return;

        var chunkCounts = SystemAPI.GetSingletonBuffer<PhysicsBodyForceChunkCount>().Reinterpret<int>().AsNativeArray();

        int total = 0;
        for (int i = 0, numChunks = chunkCounts.Length; i < numChunks; i++)
        {
            int count = chunkCounts[i];
            chunkCounts[i] = total; // replace # of changes within the chunk with it's offset
            total += count;
        }

        var forceQuery = SystemAPI.QueryBuilder().WithAll<PhysicsBodyForce, PhysicsBodyHandle>().Build();
        var batchForces = SystemAPI.GetSingletonBuffer<PhysicsBatchForce>();

        batchForces.ResizeUninitialized(total);

        var job = new ForceAggregatorJob
        {
            BodyHandle = SystemAPI.GetComponentTypeHandle<PhysicsBodyHandle>(),
            ForceHandle = SystemAPI.GetComponentTypeHandle<PhysicsBodyForce>(),
            BatchForceBuffer = batchForces.Reinterpret<PhysicsBody.BatchForce>().AsNativeArray(),
            ChunkOffsets = chunkCounts
        };

        state.Dependency = job.ScheduleParallel(forceQuery, state.Dependency);
    }

    [BurstCompile]
    partial struct ForceAggregatorJob : IJobChunk
    {
        [ReadOnly] public ComponentTypeHandle<PhysicsBodyHandle> BodyHandle;
        [ReadOnly] public NativeArray<int> ChunkOffsets;

        public ComponentTypeHandle<PhysicsBodyForce> ForceHandle;

        [NativeDisableParallelForRestriction]
        public NativeArray<PhysicsBody.BatchForce> BatchForceBuffer;

        public void Execute(
            in ArchetypeChunk chunk,
            int ChunkIndex,
            bool useEnabledMask,
            in v128 chunkEnabledMask
        )
        {
            Assert.IsFalse(useEnabledMask);
            var bodies = chunk.GetNativeArray(ref BodyHandle);
            var forces = chunk.GetNativeArray(ref ForceHandle);
            int chunkOffset = ChunkOffsets[ChunkIndex];

            for (int i = 0, forceIndex = 0, entityCount = chunk.Count; i < entityCount; i++)
            {
                if (math.any(forces[i].Force != float2.zero))
                {
                    var force = new PhysicsBody.BatchForce(bodies[i].Body);
                    force.ApplyForceToCenter(forces[i].Force);
                    forces[i] = new PhysicsBodyForce { Force = float2.zero };
                    BatchForceBuffer[chunkOffset + forceIndex] = force;
                    forceIndex++;
                }
            }
        }
    }
}
