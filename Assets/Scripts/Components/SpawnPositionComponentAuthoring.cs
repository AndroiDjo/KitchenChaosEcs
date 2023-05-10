using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class SpawnPositionComponentAuthoring : MonoBehaviour {
    [SerializeField] private GameObject spawnPosition;

    class Baker : Baker<SpawnPositionComponentAuthoring> {
        public override void Bake(SpawnPositionComponentAuthoring authoring) {
            Transform transform = GetComponent<Transform>(authoring.spawnPosition);
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpawnPositionComponent {
                Value = new LocalTransform {
                    Position = transform.position,
                    Rotation = transform.rotation,
                    Scale = 1f
                }
            });
        }
    }
}

public struct SpawnPositionComponent : IComponentData {
    public LocalTransform Value;
}