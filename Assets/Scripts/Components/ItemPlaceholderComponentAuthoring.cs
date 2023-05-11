using UnityEngine;
using Unity.Entities;

public class ItemPlaceholderComponentAuthoring : MonoBehaviour {
    [SerializeField] private GameObject placeholder;

    class Baker : Baker<ItemPlaceholderComponentAuthoring> {
        public override void Bake(ItemPlaceholderComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ItemPlaceholderComponent {
                Entity = GetEntity(authoring.placeholder, TransformUsageFlags.Dynamic)
            });
        }
    }
}
public struct ItemPlaceholderComponent : IComponentData {
    public Entity Entity;
}
