using Unity.Burst;
using Unity.Entities;
using UnityEngine.LowLevelPhysics2D;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsBatchForceCreationSystem))]
partial struct PhysicsBatchForceSubmissionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsBatchForce>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var batchForceBuffer = SystemAPI.GetSingletonBuffer<PhysicsBatchForce>();
        if (batchForceBuffer.IsEmpty) return;

        PhysicsBody.SetBatchForce(
            batchForceBuffer.Reinterpret<PhysicsBody.BatchForce>().AsNativeArray()
        );
    }
}
