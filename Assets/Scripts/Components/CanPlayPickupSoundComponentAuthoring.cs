using UnityEngine;
using Unity.Entities;

public class CanPlayPickupSoundComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanPlayPickupSoundComponentAuthoring> {
        public override void Bake(CanPlayPickupSoundComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanPlayPickupSoundComponent>(entity);
            AddComponent<MustPlayPickupSoundComponent>(entity);
            SetComponentEnabled<MustPlayPickupSoundComponent>(entity, false);
        }
    }
}

public struct CanPlayPickupSoundComponent : IComponentData {}
public struct MustPlayPickupSoundComponent : IComponentData, IEnableableComponent {}