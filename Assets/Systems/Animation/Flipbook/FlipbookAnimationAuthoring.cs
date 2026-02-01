using System;
using System.ComponentModel;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class FlipbookAnimationAuthoring : MonoBehaviour
{
    [DefaultValue(0)]
    [SerializeField] int Clip = 0;
    [DefaultValue(1f)]
    [SerializeField] float Speed = 1f;
    [SerializeField] public FlipbookClip[] Clips;
    [Serializable]
    public struct FlipbookClip
    {
        [DefaultValue("1f")]
        public float Duration;
        public int[] Frames;
    }

    class Baker : Baker<FlipbookAnimationAuthoring>
    {
        public override void Bake(FlipbookAnimationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Renderable);
            
            AddComponent(entity, new FlipbookAnimationPlayer
            {
                Clip = authoring.Clip,
                Speed = authoring.Speed,
                Progress = 0f
            });
            
            AddComponent(entity, new FlipbookAnimationMaterialOverride {
                TileIndex = 0
            });
            
            AddComponent(entity, new FlipbookAnimationClipsBlob {
                Blob = MakeAnimationBlob(authoring.Clips)
            });
        }

        BlobAssetReference<FlipbookAnimationClips> MakeAnimationBlob(FlipbookClip[] clips)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var animation = ref builder.ConstructRoot<FlipbookAnimationClips>();
            var clipArray = builder.Allocate(ref animation.Clips, clips.Length);

            for (int i = 0; i < clips.Length; i++)
            {
                var clip = clips[i];
                clipArray[i].Duration = clip.Duration == 0 ? 1f : clip.Duration;
                var indices = builder.Allocate(ref clipArray[i].Indices, clip.Frames.Length);
                for (int j = 0; j < clip.Frames.Length; j++)
                {
                    indices[j] = clip.Frames[j];
                }
            }

            var blobAssetReference = builder.CreateBlobAssetReference<FlipbookAnimationClips>(Allocator.Persistent);
            builder.Dispose();
            AddBlobAsset(ref blobAssetReference, out var hash);
            return blobAssetReference;
        }
    }
}