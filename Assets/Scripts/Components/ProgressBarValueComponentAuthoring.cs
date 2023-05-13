using UnityEngine;
using Unity.Entities;

public class ProgressBarValueComponentAuthoring : MonoBehaviour {
    class Baker : Baker<ProgressBarValueComponentAuthoring> {
        public override void Bake(ProgressBarValueComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<ProgressBarValueComponent>(entity);
        }
    }
}

public struct ProgressBarValueComponent : IComponentData {
    public float Value;
}