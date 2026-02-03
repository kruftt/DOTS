using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PhysicsForceSystemGroup))]
partial struct RepulserSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsSimulationFlag>();
        state.RequireForUpdate<InputState>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var inputState = SystemAPI.GetSingleton<InputState>();
        state.Dependency = new RepulseJob { MousePos = inputState.MousePos }.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    [WithAll(typeof(OrbitalBodyTag), typeof(PhysicsBodyHandle))]
    partial struct RepulseJob : IJobEntity
    {
        public Vector2 MousePos;

        void Execute(
            [EntityIndexInQuery] int index,
            in LocalTransform transform,
            ref PhysicsBodyForce bodyForces
        )
        {
            var direction = new float2(transform.Position.x - MousePos.x, transform.Position.y - MousePos.y);
            float coeff = math.min(80f, 12.0f / (math.square(direction.x) + math.square(direction.y)));
            bodyForces.Force += coeff * direction;
        }
    }
}
