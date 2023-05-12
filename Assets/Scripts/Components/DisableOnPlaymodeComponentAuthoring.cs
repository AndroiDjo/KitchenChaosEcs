using UnityEngine;
using Unity.Entities;

public class DisableOnPlaymodeComponentAuthoring : MonoBehaviour {
    class Baker : Baker<DisableOnPlaymodeComponentAuthoring> {
        public override void Bake(DisableOnPlaymodeComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<DisableOnPlaymodeComponent>(entity);
        }
    }
}
public struct DisableOnPlaymodeComponent : IComponentData {}