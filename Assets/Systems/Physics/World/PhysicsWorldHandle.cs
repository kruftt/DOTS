using Unity.Entities;
using UnityEngine.LowLevelPhysics2D;

public struct PhysicsWorldHandle : IComponentData
{
    public PhysicsWorld World;
}
