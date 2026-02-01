using Unity.Entities;
using UnityEngine;

class TransientTickerAuthoring : MonoBehaviour
{
    class Baker : Baker<TransientTickerAuthoring>
    {
        public override void Bake(TransientTickerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new TransientTicker
            {
                Lifetime = 1,
                EmitEvent = false
            });
        }
    }
}