using Unity.Burst;
using Unity.Entities;
using UnityEngine.LowLevelPhysics2D;

[UpdateInGroup(typeof(PhysicsForcePassSystemGroup))]
[UpdateAfter(typeof(PhysicsBatchForceCreationSystem))]
partial struct PhysicsBatchForceSubmissionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsBatchForce>();
        state.RequireForUpdate<PhysicsSimulationFlag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var app = SystemAPI.GetSingletonEntity<AppTag>();
        if (SystemAPI.IsComponentEnabled<PhysicsSimulationFlag>(app)) return;

        SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged)
            .SetComponentEnabled<PhysicsSimulationFlag>(app, true);

        var batchForceBuffer = SystemAPI.GetSingletonBuffer<PhysicsBatchForce>();
        if (batchForceBuffer.IsEmpty) return;

        PhysicsBody.SetBatchForce(
            batchForceBuffer.Reinterpret<PhysicsBody.BatchForce>().AsNativeArray()
        );
    }
}
