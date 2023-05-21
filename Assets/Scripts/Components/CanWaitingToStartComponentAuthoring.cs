using UnityEngine;
using Unity.Entities;

public class CanWaitingToStartComponentAuthoring : MonoBehaviour {
    public float Goal;
    
    class Baker : Baker<CanWaitingToStartComponentAuthoring> {
        public override void Bake(CanWaitingToStartComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanWaitingToStartComponent>(entity);
            AddComponent(entity, new WaitingToStartComponent{ Goal = authoring.Goal });
            SetComponentEnabled<WaitingToStartComponent>(entity, false);
        }
    }
}
public struct CanWaitingToStartComponent : IComponentData {}

public struct WaitingToStartComponent : IComponentData, IEnableableComponent {
    public float Goal;
    public float Timer;
}