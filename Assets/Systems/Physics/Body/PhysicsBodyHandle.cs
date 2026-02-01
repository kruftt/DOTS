using Unity.Entities;
using UnityEngine.LowLevelPhysics2D;

public struct PhysicsBodyHandle : IComponentData
{
    public PhysicsBody Body;
}
