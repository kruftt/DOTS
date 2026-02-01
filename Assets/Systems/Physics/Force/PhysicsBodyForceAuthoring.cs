using Unity.Entities;
using UnityEngine;

class PhysicsBodyForceAuthoring : MonoBehaviour
{
    class Baker : Baker<PhysicsBodyForceAuthoring>
    {
        public override void Bake(PhysicsBodyForceAuthoring authoring)
        {
            AddComponent<PhysicsBodyForce>(GetEntity(TransformUsageFlags.None));
        }
    }
}