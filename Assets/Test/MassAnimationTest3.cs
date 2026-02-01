using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class MassAnimationTest3Authoring : MonoBehaviour
{
    [SerializeField] GameObject Prefab;
    [SerializeField] PhysicsBodyDefinition Definition;
    [SerializeField] PhysicsShapeDefinition[] Shapes;

    class Baker : Baker<MassAnimationTest3Authoring>
    {
        public override void Bake(MassAnimationTest3Authoring authoring)
        {
            var random = new Unity.Mathematics.Random();
            random.InitState(26246254);

            for (int i = 0; i < 10000; i++)
            {
                Entity entity = CreateAdditionalEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PrefabInstanceRequest { IsStatic = false });
                AddComponent(entity, new RequestEntityPrefabLoaded { Prefab = new EntityPrefabReference(authoring.Prefab) });

                float magnitude = random.NextFloat(0f, 3f);
                float angle = random.NextFloat(0f, math.TAU);
                var position = new float3(
                    magnitude * math.cos(angle),
                    magnitude * math.sin(angle),
                    random.NextFloat(0f, 100f)
                );

                float scale = random.NextFloat(0.25f, 0.5f);
                AddComponent(entity, new TransformInitData
                {
                    Transform = new LocalTransform
                    {
                        Position = position,
                        Scale = scale,
                        Rotation = quaternion.identity
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

                var definition = authoring.Definition;
                definition.position = new float2(position.x, position.y);
                definition.linearVelocity = random.NextFloat2(-2f, 2f);

                AddComponent(entity, new PhysicsBodyInitData
                {
                    Definition = definition
                });

                var shapes = AddBuffer<PhysicsBodyShapes>(entity);
                foreach (var shape in authoring.Shapes)
                {
                    var _shape = shape;
                    _shape.density = 4.0f * scale;
                    shapes.Add(new PhysicsBodyShapes
                    {
                        Shape = _shape
                    });
                }
            }
        }
    }
}