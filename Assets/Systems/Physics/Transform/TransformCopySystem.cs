using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.LowLevelPhysics2D;
using static UnityEngine.LowLevelPhysics2D.PhysicsEvents;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct TransformCopySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldHandle>();
        state.RequireForUpdate<PhysicsTransformPassFlag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var app = SystemAPI.GetSingletonEntity<AppTag>();
        if (!SystemAPI.IsComponentEnabled<PhysicsTransformPassFlag>(app)) return;

        SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged)
            .SetComponentEnabled<PhysicsTransformPassFlag>(app, false);

        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldHandle>().World;
        if (physicsWorld == null || !physicsWorld.isValid)
            return;

        int numUpdates = physicsWorld.bodyUpdateEvents.Length;
        if (numUpdates == 0)
            return;

        NativeArray<BodyUpdateEvent> bodyUpdateEvents = new(
            numUpdates,
            Allocator.TempJob,
            NativeArrayOptions.UninitializedMemory
        );

        physicsWorld.bodyUpdateEvents.CopyTo(bodyUpdateEvents.AsSpan());

        var job = new CopyJob
        {
            BodyUpdateEvents = bodyUpdateEvents,
            UserData = physicsWorld.GetBodyUpdateUserData(Allocator.TempJob),
            TransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(),
        };

        state.Dependency = job.Schedule(numUpdates, numUpdates / 8, state.Dependency);
    }

    [BurstCompile]
    partial struct CopyJob : IJobParallelFor
    {
        [ReadOnly]
        [DeallocateOnJobCompletion]
        public NativeArray<BodyUpdateEvent> BodyUpdateEvents;

        [ReadOnly]
        [DeallocateOnJobCompletion]
        public NativeArray<PhysicsUserData> UserData;

        [NativeDisableParallelForRestriction]
        public ComponentLookup<LocalTransform> TransformLookup;

        public void Execute(int index)
        {
            var update = BodyUpdateEvents[index];
            var entity = UserData[index].int64Value.ToEntity();
            var transform = TransformLookup[entity];
            var pose = update.transform.position;
            transform.Position = new float3(pose.x, pose.y, transform.Position.z);
            TransformLookup[entity] = transform;
        }
    }
}
