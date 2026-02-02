using System.Xml.Serialization;
using Unity.Burst;
using Unity.Entities;


[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct PhysicsSimulationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AppTag>();
        state.RequireForUpdate<PhysicsWorldHandle>();
        state.RequireForUpdate<PhysicsSimulationFlag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var app = SystemAPI.GetSingletonEntity<AppTag>();
        if (!SystemAPI.IsComponentEnabled<PhysicsSimulationFlag>(app)) return;

        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        ecb.SetComponentEnabled<PhysicsSimulationFlag>(app, false);
        ecb.SetComponentEnabled<PhysicsTransformPassFlag>(app, true);

        SystemAPI.GetSingleton<PhysicsWorldHandle>().World
            .Simulate(state.World.GetExistingSystemManaged<FixedStepSimulationSystemGroup>().Timestep);
    }
}
