using UnityEngine;
using Unity.Entities;

public class NeedUpdateUIComponentAuthoring : MonoBehaviour {
    class Baker : Baker<NeedUpdateUIComponentAuthoring> {
        public override void Bake(NeedUpdateUIComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<NeedUpdateUIComponent>(entity);
            SetComponentEnabled<NeedUpdateUIComponent>(entity, false);
        }
    }
}
public struct NeedUpdateUIComponent : IComponentData, IEnableableComponent {}