using Unity.Entities;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

class PhysicsWorldAuthoring : MonoBehaviour
{
    [SerializeField] PhysicsWorldDefinition Definition = PhysicsWorldDefinition.defaultDefinition;

    class Baker : Baker<PhysicsWorldAuthoring>
    {
        public override void Bake(PhysicsWorldAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(
                entity,
                new PhysicsWorldInitData
                {
                    Definition = authoring.Definition
                }
            );
        }
    }
}
