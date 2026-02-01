using Unity.Entities;
using UnityEngine.LowLevelPhysics2D;

public struct PhysicsBodyShapes : IBufferElementData
{
    public PhysicsShapeDefinition Shape;
}
