using UnityEngine;
using Unity.Entities;

public class CanPlayDropSoundComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanPlayDropSoundComponentAuthoring> {
        public override void Bake(CanPlayDropSoundComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanPlayDropSoundComponent>(entity);
            AddComponent<MustPlayDropSoundComponent>(entity);
            SetComponentEnabled<MustPlayDropSoundComponent>(entity, false);
        }
    }
}

public struct CanPlayDropSoundComponent : IComponentData {}
public struct MustPlayDropSoundComponent : IComponentData, IEnableableComponent {}