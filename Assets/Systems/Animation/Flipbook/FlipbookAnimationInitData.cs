using System;
using Unity.Entities;

[Serializable]
public struct FlipbookAnimationInitData : IComponentData
{
    public int Clip;
    public float Speed;
}
