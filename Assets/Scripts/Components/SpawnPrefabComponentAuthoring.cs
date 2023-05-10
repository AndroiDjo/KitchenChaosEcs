using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class SpawnPrefabComponentAuthoring : MonoBehaviour {
    [SerializeField] private GameObject prefab;

    class Baker : Baker<SpawnPrefabComponentAuthoring> {
        public override void Bake(SpawnPrefabComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpawnPrefabComponent {
                Prefab = GetEntity(authoring.prefab, TransformUsageFlags.None)
            });
        }
    }
}

public struct SpawnPrefabComponent : IComponentData {
    public Entity Prefab;
}