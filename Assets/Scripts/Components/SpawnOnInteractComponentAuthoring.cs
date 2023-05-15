using UnityEngine;
using Unity.Entities;

public class SpawnOnInteractComponentAuthoring : MonoBehaviour {
    class Baker : Baker<SpawnOnInteractComponentAuthoring> {
        public override void Bake(SpawnOnInteractComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<SpawnOnInteractComponent>(entity);
        }
    }
}

public struct SpawnOnInteractComponent : IComponentData {}