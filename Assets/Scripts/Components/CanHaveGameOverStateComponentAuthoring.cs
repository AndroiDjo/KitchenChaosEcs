using UnityEngine;
using Unity.Entities;

public class CanHaveGameOverStateComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanHaveGameOverStateComponentAuthoring> {
        public override void Bake(CanHaveGameOverStateComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanHaveGameOverStateComponent>(entity);
            AddComponent<IsGameOverStateComponent>(entity);
            SetComponentEnabled<IsGameOverStateComponent>(entity, false);
            AddComponent<NeedShowGameOverUIComponent>(entity);
            SetComponentEnabled<NeedShowGameOverUIComponent>(entity, false);
            AddComponent<GameOverUIAreShownComponent>(entity);
            SetComponentEnabled<GameOverUIAreShownComponent>(entity, false);
        }
    }
}

public struct CanHaveGameOverStateComponent : IComponentData {}
public struct IsGameOverStateComponent : IComponentData, IEnableableComponent {}

public struct NeedShowGameOverUIComponent : IComponentData, IEnableableComponent {
    public int SuccessOrders;
}
public struct GameOverUIAreShownComponent : IComponentData, IEnableableComponent {}