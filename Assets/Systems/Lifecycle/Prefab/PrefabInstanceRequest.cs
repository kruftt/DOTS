using System.ComponentModel;
using Unity.Entities;

public struct PrefabInstanceRequest : IComponentData
{
    [DefaultValue(false)]
    public bool IsStatic;
}
