using Unity.Entities;
using UnityEngine.LowLevelPhysics2D;

public struct PhysicsWorldInitData : IComponentData
{
    public PhysicsWorldDefinition Definition;
}
