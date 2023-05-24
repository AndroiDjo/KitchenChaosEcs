using UnityEngine;
using Unity.Entities;

public class CanPlayWarningSoundComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanPlayWarningSoundComponentAuthoring> {
        public override void Bake(CanPlayWarningSoundComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanPlayWarningSoundComponent>(entity);
        }
    }
}

public struct CanPlayWarningSoundComponent : IComponentData {}