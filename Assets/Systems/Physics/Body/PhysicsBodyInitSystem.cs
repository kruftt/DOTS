using Unity.Burst;
using Unity.Entities;
using UnityEngine.LowLevelPhysics2D;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(PhysicsWorldInitSystem))]
partial struct PhysicsBodyInitSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldHandle>();
        state.RequireForUpdate(SystemAPI.QueryBuilder()
            .WithAll<EntityInstanceInitRequest, EntityInstance, PhysicsBodyInitData>()
            .Build()
        );
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var world = SystemAPI.GetSingleton<PhysicsWorldHandle>().World;
        if (world == null || !world.isValid) return;

        var ecb = SystemAPI
            .GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (entityInstance, data, entity) in SystemAPI.Query<
            RefRO<EntityInstance>,
            RefRO<PhysicsBodyInitData>>()
            .WithAll<EntityInstanceInitRequest>()
            .WithEntityAccess()
        )
        {
            var buffer = SystemAPI.GetBuffer<PhysicsBodyShapes>(entity);

            var instance = entityInstance.ValueRO.Value;
            var body = world.CreateBody(data.ValueRO.Definition);
            foreach (var shape in buffer)
            {
                body.CreateShape(CircleGeometry.defaultGeometry, shape.Shape);
            }

            body.userData = new PhysicsUserData
            {
                int64Value = instance.ToUInt64()
            };

            var handle = SystemAPI.GetComponentRW<PhysicsBodyHandle>(instance);
            handle.ValueRW.Body = body;
            ecb.DestroyEntity(entity);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
