using UnityEngine;
using Unity.Entities;

public class SlicedEntityPrefabComponentAuthoring : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    class Baker : Baker<SlicedEntityPrefabComponentAuthoring> {
        public override void Bake(SlicedEntityPrefabComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SlicedEntityPrefabComponent {
                Prefab = GetEntity(authoring.prefab, TransformUsageFlags.None)
            });
        }
    }
}

public struct SlicedEntityPrefabComponent : IComponentData {
    public Entity Prefab;
}