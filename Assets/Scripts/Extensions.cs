using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

public static class Extensions
{
    public static ulong ToUInt64(this Entity entity)
        => UnsafeUtility.As<Entity, ulong>(ref entity);

    public static Entity ToEntity(this ulong value)
        => UnsafeUtility.As<ulong, Entity>(ref value);
}
