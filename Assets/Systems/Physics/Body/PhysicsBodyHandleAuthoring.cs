using Unity.Entities;
using UnityEngine;

class PhysicsBodyHandleAuthoring : MonoBehaviour
{
    class Baker : Baker<PhysicsBodyHandleAuthoring>
    {
        public override void Bake(PhysicsBodyHandleAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PhysicsBodyHandle>(entity);
        }
    }
}