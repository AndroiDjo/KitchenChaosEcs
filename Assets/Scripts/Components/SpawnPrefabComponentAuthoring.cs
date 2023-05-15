using UnityEngine;
using Unity.Entities;

public class SpawnPrefabComponentAuthoring : MonoBehaviour {
    public GameObject Prefab;
    public float EntityHeight;

    class Baker : Baker<SpawnPrefabComponentAuthoring> {
        public override void Bake(SpawnPrefabComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpawnPrefabComponent {
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.None),
                EntityHeight = authoring.EntityHeight
            });
        }
    }
}

public struct SpawnPrefabComponent : IComponentData {
    public Entity Prefab;
    public float EntityHeight;
}