using UnityEngine;
using Unity.Entities;

public class CanWaitingToStartComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanWaitingToStartComponentAuthoring> {
        public override void Bake(CanWaitingToStartComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<WaitingToStartComponent>(entity);
        }
    }
}

public struct WaitingToStartComponent : IComponentData {
}