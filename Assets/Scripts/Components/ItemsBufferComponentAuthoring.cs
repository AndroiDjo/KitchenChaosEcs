using Unity.Entities;
using UnityEngine;

public class ItemsBufferComponentAuthoring : MonoBehaviour {
    class Baker : Baker<ItemsBufferComponentAuthoring> {
        public override void Bake(ItemsBufferComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddBuffer<ItemsBufferComponent>(entity);
        }
    }
}

public struct ItemsBufferComponent : IBufferElementData {
    public Entity Item;
}