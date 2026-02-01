using Unity.Entities;

public struct FlipbookAnimationPlayer : IComponentData
{
    public int Clip;
    public float Progress;
    public float Speed;
}
