using Unity.Entities;
using UnityEngine.LowLevelPhysics2D;

public struct PhysicsBodyInitData : IComponentData
{
    public PhysicsBodyDefinition Definition;
}
