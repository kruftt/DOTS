using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PhysicsForceSystemGroup))]
partial struct OrbitalBodySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsSimulationFlag>();
        state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<PhysicsBodyHandle, PhysicsBodyForce, OrbitalBodyTag>()
                .Build());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var inputState = SystemAPI.GetSingleton<InputState>();

        state.Dependency = new OrbitalBodyJob
        {
            MousePos = inputState.MousePos,
            DeltaTime = Time.deltaTime,
        }.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    [WithAll(typeof(OrbitalBodyTag), typeof(PhysicsBodyHandle))]
    partial struct OrbitalBodyJob : IJobEntity
    {
        public Vector2 MousePos;
        public float DeltaTime;

        void Execute(
            [EntityIndexInQuery] int index,
            in LocalTransform transform,
            ref PhysicsBodyForce bodyForces
        )
        {
            bodyForces.Force -= new float2(transform.Position.x, transform.Position.y);
        }
    }
}
