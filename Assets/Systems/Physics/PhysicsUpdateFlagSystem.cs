using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
partial struct PhysicsUpdateFlagSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldHandle>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI
            .GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        var entity = ecb.CreateEntity();
        ecb.AddComponent<PhysicsUpdateFlag>(entity);
        ecb.AddComponent<TransientTicker>(entity);
        ecb.SetComponent(entity, new TransientTicker
        {
            Lifetime = 1,
            EmitEvent = false,
        });
    }
}
