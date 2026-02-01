using System;
using Unity.Entities;

[Serializable]
public struct TransientTimer : IComponentData
{
    public float Lifetime;
    public bool EmitEvent;
}
