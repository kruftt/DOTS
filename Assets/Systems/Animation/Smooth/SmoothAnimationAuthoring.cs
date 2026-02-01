using Unity.Entities;
using UnityEngine;

class SmoothAnimationAuthoring : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Duration;
    [SerializeField] bool Reverse;
    [SerializeField] bool Loop;
    [SerializeField] bool Alternate;

    class Baker : Baker<SmoothAnimationAuthoring>
    {
        public override void Bake(SmoothAnimationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Renderable);

            AddComponent(entity, new SmoothAnimationPlayer
            {
                Speed = authoring.Speed,
                Duration = authoring.Duration,
                Progress = 0f,
                Reverse = authoring.Reverse,
                Loop = authoring.Loop,
                Alternate = authoring.Alternate
            });

            AddComponent<SmoothAnimationMaterialOverride>(entity);
        }
    }
}