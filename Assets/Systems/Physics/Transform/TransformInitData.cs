using Unity.Entities;
using Unity.Transforms;

public struct TransformInitData : IComponentData
{
    public LocalTransform Transform;
}
