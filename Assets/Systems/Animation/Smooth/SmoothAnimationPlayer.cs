using System;
using Unity.Entities;

[Serializable]
public struct SmoothAnimationPlayer : IComponentData
{
    public float Speed;
    public float Duration;
    public float Progress;
    public bool Reverse;
    public bool Loop;
    public bool Alternate;
}
