using Unity.Entities;
using static UnityEngine.LowLevelPhysics2D.PhysicsBody;

public struct PhysicsBatchForce : IBufferElementData
{
    public BatchForce Force;
}
