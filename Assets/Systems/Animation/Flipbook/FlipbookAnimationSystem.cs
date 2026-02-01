using Unity.Burst;
using Unity.Entities;

partial struct FlipbookAnimationSystem : ISystem
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
            ref FlipbookAnimationMaterialOverride matOverride,
            ref FlipbookAnimationClipsBlob clips,
            ref FlipbookAnimationPlayer player
        )
        {
            ref var blob = ref clips.Blob.Value;
            ref var clip = ref blob.Clips[player.Clip];
            player.Progress = (player.Progress + (dt * player.Speed / clip.Duration)) % 0.9999999f;
            matOverride.TileIndex = clip.Indices[(int)(player.Progress * clip.Indices.Length)];
        }
    }
}