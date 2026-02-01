using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsForceChunkingSystem))]
partial struct OrbitalBodySystem : ISystem
{
    EntityQuery orbitalQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        orbitalQuery = SystemAPI.QueryBuilder()
                .WithAll<PhysicsBodyHandle, PhysicsBodyForce, OrbitalBodyTag>()
                .Build();

        state.RequireForUpdate(orbitalQuery);
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
    [WithAll(typeof(OrbitalBodyTag))]
    partial struct OrbitalBodyJob : IJobEntity
    {
        public Vector2 MousePos;
        public float DeltaTime;

        void Execute(
            [EntityIndexInQuery] int index,
            in PhysicsBodyHandle bodyHandle,
            ref PhysicsBodyForce bodyForces
        )
        {
            bodyForces.Force -= (float2)bodyHandle.Body.position;
        }
    }
}
