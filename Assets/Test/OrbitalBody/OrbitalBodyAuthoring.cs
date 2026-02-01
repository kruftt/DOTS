using Unity.Entities;
using UnityEngine;

class OrbitalBodyAuthoring : MonoBehaviour
{
    class Baker : Baker<OrbitalBodyAuthoring>
    {
        public override void Bake(OrbitalBodyAuthoring authoring)
        {
            AddComponent(
                GetEntity(TransformUsageFlags.Dynamic),
                new OrbitalBodyTag { }
            );
        }
    }
}