using System;
using Unity.Entities;

public struct FlipbookAnimationClipsBlob : IComponentData
{
    public BlobAssetReference<FlipbookAnimationClips> Blob;
}

public struct FlipbookAnimationClips
{
    public BlobArray<FlipbookAnimationClip> Clips;
}

[Serializable]
public struct FlipbookAnimationClip
{
    public float Duration;
    public BlobArray<float> Indices;
}
