using UnityEngine;
using Unity.Entities;

public class SpawnItemsOverTimeComponentAuthoring : MonoBehaviour {
    public float SpawnAfter;
    public int ItemsLimit;
    class Baker : Baker<SpawnItemsOverTimeComponentAuthoring> {
        public override void Bake(SpawnItemsOverTimeComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpawnItemsOverTimeComponent {
                SpawnAfter = authoring.SpawnAfter,
                ItemsLimit = authoring.ItemsLimit
            });
        }
    }
}
public struct SpawnItemsOverTimeComponent : IComponentData {
    public float SpawnAfter;
    public float Timer;
    public int ItemsLimit;
}