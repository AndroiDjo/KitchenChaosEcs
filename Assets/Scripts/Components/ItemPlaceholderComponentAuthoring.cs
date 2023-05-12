using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ItemPlaceholderComponentAuthoring : MonoBehaviour {
    [SerializeField] private GameObject placeholder;
    [SerializeField] private float3 localPosition = float3.zero;
    [SerializeField] private quaternion localRotation = quaternion.identity;
    [SerializeField] private float localScale = 1f;

    class Baker : Baker<ItemPlaceholderComponentAuthoring> {
        public override void Bake(ItemPlaceholderComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ItemPlaceholderComponent {
                Entity = GetEntity(authoring.placeholder, TransformUsageFlags.Dynamic),
                LocalPosition = new LocalTransform {
                    Position = authoring.localPosition,
                    Rotation = authoring.localRotation,
                    Scale = authoring.localScale
                }
            });
        }
    }
}
public struct ItemPlaceholderComponent : IComponentData {
    public Entity Entity;
    public LocalTransform LocalPosition;
}
