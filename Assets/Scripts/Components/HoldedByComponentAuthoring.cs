using Unity.Entities;
using UnityEngine;

public class HoldedByComponentAuthoring : MonoBehaviour {
    class Baker : Baker<HoldedByComponentAuthoring> {
        public override void Bake(HoldedByComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<HoldedByComponent>(entity);
        }
    }
}

public struct HoldedByComponent : IComponentData {
    public HolderType HolderType;
}