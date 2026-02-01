using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(PhysicsWorldInitSystem))]
[UpdateBefore(typeof(PhysicsBodyInitSystem))]
partial struct TransformInitSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(
            SystemAPI.QueryBuilder()
                .WithAll<EntityInstance, EntityInstanceInitRequest, TransformInitData>()
                .Build()
        );
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var em = state.EntityManager;

        foreach (var (request, instance, transform) in SystemAPI.Query
            <RefRO<PrefabInstanceRequest>,
            RefRO<EntityInstance>,
            RefRO<TransformInitData>>())
        {
            if (request.ValueRO.IsStatic)
            {
                var t = transform.ValueRO.Transform;
                em.SetComponentData(instance.ValueRO.Value, new LocalToWorld
                {
                    Value = Matrix4x4.TRS(t.Position, t.Rotation, new float3(t.Scale))
                });
            }
            else
            {
                em.SetComponentData(instance.ValueRO.Value, transform.ValueRO.Transform);
            }
        }
    }
}
