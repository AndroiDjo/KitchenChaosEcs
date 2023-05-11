using UnityEngine;
using Unity.Entities;

public class InteractedPlayerItemPlaceholderComponentAuthoring : MonoBehaviour {
    class Baker : Baker<InteractedPlayerItemPlaceholderComponentAuthoring> {
        public override void Bake(InteractedPlayerItemPlaceholderComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<InteractedPlayerItemPlaceholderComponent>(entity);
        }
    }
}

public struct InteractedPlayerItemPlaceholderComponent : IComponentData {
    public ItemPlaceholderComponent Placeholder;
}