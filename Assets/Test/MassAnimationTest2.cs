using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;

public class MassAnimationTest2Authoring : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    class Baker : Baker<MassAnimationTest2Authoring>
    {
        public override void Bake(MassAnimationTest2Authoring authoring)
        {
            var random = new Unity.Mathematics.Random();
            random.InitState(26246254);

            for (int i = 0; i < 10000; i++)
            {
                Entity entity = CreateAdditionalEntity(TransformUsageFlags.None);

                AddComponent(entity, new PrefabInstanceRequest { IsStatic = true });
                AddComponent(entity, new RequestEntityPrefabLoaded
                {
                    Prefab = new EntityPrefabReference(authoring.prefab)
                });

                AddComponent(entity, new TransformInitData
                {
                    Transform = new LocalTransform()
                    {
                        Position = new Unity.Mathematics.float3(
                            random.NextFloat(-9f, 9f),
                            random.NextFloat(-5f, 5f),
                            random.NextFloat(0f, 100f)
                        ),
                        Scale = random.NextFloat(0.25f, 0.42f),
                        Rotation = Unity.Mathematics.quaternion.identity
                    }
                });

                AddComponent(entity, new SmoothAnimationInitData
                {
                    Speed = random.NextFloat(0.5f, 2.0f),
                    Duration = random.NextFloat(2.0f, 8.0f),
                    Reverse = random.NextBool(),
                    Loop = true,
                    Alternate = true
                });
            }
        }
    }
}