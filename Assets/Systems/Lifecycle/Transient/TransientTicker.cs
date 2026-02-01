using System;
using Unity.Entities;

[Serializable]
public struct TransientTicker : IComponentData
{
    public int Lifetime;
    public bool EmitEvent;
}
