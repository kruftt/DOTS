using Unity.Entities;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

class PhysicsBodyInitAuthoring : MonoBehaviour
{
    [SerializeField] PhysicsBodyDefinition Definition;
    [SerializeField] PhysicsShapeDefinition[] Shapes;

    class Baker : Baker<PhysicsBodyInitAuthoring>
    {
        public override void Bake(PhysicsBodyInitAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            var initRequest = CreateAdditionalEntity(TransformUsageFlags.None);
            AddComponent(initRequest, new EntityInstanceInitRequest { });
            AddComponent(initRequest, new EntityInstance { Value = entity });
            AddComponent(initRequest, new PhysicsBodyInitData
            {
                Definition = authoring.Definition
            });

            var shapes = AddBuffer<PhysicsBodyShapes>(initRequest);
            foreach (var shape in authoring.Shapes)
            {
                shapes.Add(new PhysicsBodyShapes
                {
                    Shape = shape
                });
            }
        }
    }
}