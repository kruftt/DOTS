using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;

public class MassAnimationTestAuthoring : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    class Baker : Baker<MassAnimationTestAuthoring>
    {
        public override void Bake(MassAnimationTestAuthoring authoring)
        {
            GetEntity(TransformUsageFlags.None);

            var random = new Unity.Mathematics.Random();
            random.InitState(26246254);

            for (int i = 0; i < 10000; i++)
            {
                Entity entity = CreateAdditionalEntity(TransformUsageFlags.None);

                AddComponent(entity, new PrefabInstanceRequest
                {
                    IsStatic = true,
                });

                AddComponent(entity, new RequestEntityPrefabLoaded
                {
                    Prefab = new EntityPrefabReference(authoring.prefab)
                });

                AddComponent(entity, new TransformInitData
                {
                    Transform = new LocalTransform
                    {
                        Position = new float3(
                            random.NextFloat(-9f, 9f),
                            random.NextFloat(-5f, 5f),
                            0f
                            ),
                        Rotation = quaternion.identity,
                        Scale = random.NextFloat(0.25f, 0.42f)
                    }
                });

                AddComponent(entity, new FlipbookAnimationInitData
                {
                    Clip = random.NextInt(0, 2),
                    Speed = random.NextFloat(0.5f, 2.0f),
                });
            }
        }
    }
}