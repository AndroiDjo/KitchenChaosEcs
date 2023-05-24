using UnityEngine;
using Unity.Entities;

public class WarningOnLimitComponentAuthoring : MonoBehaviour {
    public float WarningOnLimit;
    class Baker : Baker<WarningOnLimitComponentAuthoring> {
        public override void Bake(WarningOnLimitComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new WarningOnLimitComponent { WarningOnLimit = authoring.WarningOnLimit });
        }
    }
}

public struct WarningOnLimitComponent : IComponentData {
    public float WarningOnLimit;
}