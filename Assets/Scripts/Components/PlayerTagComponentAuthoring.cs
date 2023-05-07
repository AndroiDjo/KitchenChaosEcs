using Unity.Entities;
using UnityEngine;

public class PlayerTagComponentAuthoring : MonoBehaviour
{
    class Baker : Baker<PlayerTagComponentAuthoring> {
        public override void Bake(PlayerTagComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerTagComponent>(entity);
        }
    }
}

public struct PlayerTagComponent : IComponentData {}