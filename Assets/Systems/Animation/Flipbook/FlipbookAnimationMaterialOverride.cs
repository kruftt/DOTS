using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_TileIndex")]
public struct FlipbookAnimationMaterialOverride : IComponentData
{
    public float TileIndex;
}
