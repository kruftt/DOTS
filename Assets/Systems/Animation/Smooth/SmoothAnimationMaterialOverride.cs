using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_Progress")]
public struct SmoothAnimationMaterialOverride : IComponentData
{
    public float Progress;
}
