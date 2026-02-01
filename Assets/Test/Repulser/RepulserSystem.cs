using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsForceChunkingSystem))]
partial struct RepulserSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<InputState>().Build());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var inputState = SystemAPI.GetSingleton<InputState>();
        state.Dependency = new RepulseJob { MousePos = inputState.MousePos }.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    [WithAll(typeof(OrbitalBodyTag))]
    partial struct RepulseJob : IJobEntity
    {
        public Vector2 MousePos;

        void Execute(
            [EntityIndexInQuery] int index,
            in PhysicsBodyHandle bodyHandle,
            ref PhysicsBodyForce bodyForces
        )
        {
            Vector2 direction = (bodyHandle.Body.position - MousePos);
            float distSquared = direction.sqrMagnitude;
            bodyForces.Force += (float2)(5.0f * direction / distSquared);
        }
    }
}
