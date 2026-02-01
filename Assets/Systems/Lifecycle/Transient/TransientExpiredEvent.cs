using Unity.Entities;

public struct TransientExpiredEvent : IComponentData
{
    public Entity Entity;
}
