using UnityEngine;
using Unity.Entities;

public class CanCountdownToStartComponentAuthoring : MonoBehaviour {
    public float Goal;
    
    class Baker : Baker<CanCountdownToStartComponentAuthoring> {
        public override void Bake(CanCountdownToStartComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanCountdownToStartComponent>(entity);
            AddComponent(entity, new CountdownToStartComponent{ Goal = authoring.Goal });
            SetComponentEnabled<CountdownToStartComponent>(entity, false);
            AddComponent<CountdownFinishedComponent>(entity);
            SetComponentEnabled<CountdownFinishedComponent>(entity, false);
        }
    }
}
public struct CanCountdownToStartComponent : IComponentData {}

public struct CountdownToStartComponent : IComponentData, IEnableableComponent {
    public float Goal;
    public float Timer;
}

public struct CountdownFinishedComponent : IComponentData, IEnableableComponent {}