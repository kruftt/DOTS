using Unity.Burst;
using Unity.Entities;

partial struct SmoothAnimationSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new AnimationJob { dt = SystemAPI.Time.DeltaTime }.ScheduleParallel();
    }

    [BurstCompile]
    partial struct AnimationJob : IJobEntity
    {
        public float dt;
        void Execute(
            ref SmoothAnimationMaterialOverride matOverride,
            ref SmoothAnimationPlayer player
        )
        {
            float duration = player.Alternate ? player.Duration * 2f : player.Duration;
            float progress = player.Progress + (dt * player.Speed / duration);

            if (progress >= 1f)
            {
                if (player.Loop)
                {
                    progress -= 1f;
                }
                else
                {
                    progress = 1f;
                }
            }

            player.Progress = progress;

            if (player.Alternate)
            {
                progress *= 2f;
                if (progress > 1f)
                {
                    progress = 2f - progress;
                }
            }

            if (player.Reverse)
            {
                progress = 1f - progress;
            }

            matOverride.Progress = progress;
        }
    }
}
