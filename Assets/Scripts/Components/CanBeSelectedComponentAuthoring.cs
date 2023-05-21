using UnityEngine;
using Unity.Entities;

public class CanBeSelectedComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanBeSelectedComponentAuthoring> {
        public override void Bake(CanBeSelectedComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanBeSelectedComponent>(entity);
            AddComponent<IsSelectedItemComponent>(entity);
            SetComponentEnabled<IsSelectedItemComponent>(entity,false);
            AddComponent<IsSelectionRestricted>(entity);
            SetComponentEnabled<IsSelectionRestricted>(entity,false);
        }
    }
}

public struct CanBeSelectedComponent : IComponentData {}

public struct IsSelectedItemComponent: IComponentData, IEnableableComponent {}
public struct IsSelectionRestricted : IComponentData, IEnableableComponent {}