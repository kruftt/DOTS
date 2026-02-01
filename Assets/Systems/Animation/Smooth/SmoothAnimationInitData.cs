using System;
using Unity.Entities;

[Serializable]
public struct SmoothAnimationInitData : IComponentData
{
    public float Speed;
    public float Duration;
    public bool Reverse;
    public bool Loop;
    public bool Alternate;
}
