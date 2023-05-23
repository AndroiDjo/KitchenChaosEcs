using UnityEngine;
using Unity.Entities;

public class SpawnPlatesOverTimeComponentAuthoring : MonoBehaviour {
    public float SpawnAfter;
    public int ItemsLimit;
    class Baker : Baker<SpawnPlatesOverTimeComponentAuthoring> {
        public override void Bake(SpawnPlatesOverTimeComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpawnPlatesOverTimeComponent {
                SpawnAfter = authoring.SpawnAfter,
                ItemsLimit = authoring.ItemsLimit
            });
            AddComponent<IsSpawnPlatesRestricted>(entity);
            SetComponentEnabled<IsSpawnPlatesRestricted>(entity, false);
        }
    }
}
public struct SpawnPlatesOverTimeComponent : IComponentData {
    public float SpawnAfter;
    public float Timer;
    public int ItemsLimit;
}

public struct IsSpawnPlatesRestricted : IComponentData, IEnableableComponent {}